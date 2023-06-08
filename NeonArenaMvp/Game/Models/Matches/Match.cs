using System.Dynamic;
using NeonArenaMvp.Game.Models.Actions;
using NeonArenaMvp.Game.Models.Events;
using NeonArenaMvp.Network.Models;
using NeonArenaMvp.Game.Models.Maps;
using NeonArenaMvp.Game.Models.Players;
using NeonArenaMvp.Game.Helpers.Builders;
using NeonArenaMvp.Game.Systems;

using static NeonArenaMvp.Game.Models.Events.MatchEventHandler;
using static NeonArenaMvp.Game.Helpers.Builders.TileBuilders;
using static NeonArenaMvp.Game.Models.Events.MatchEvent;
using static NeonArenaMvp.Game.Helpers.Models.Constants;
using static NeonArenaMvp.Game.Systems.Helpers.SystemHelpers;
using NeonArenaMvp.Network.Models.Dto.Step;
using NeonArenaMvp.Network.Services.Interfaces;
using static NeonArenaMvp.Game.Helpers.Models.Directions;

namespace NeonArenaMvp.Game.Models.Matches
{
    public class Match
    {
        public string ParentLobbyId;
        public Map Map;
        public List<Player> Players;
        public List<Character> Characters;
        public GameMode GameMode;

        public int WinningTeam = NeutralTeam;
        public bool HasWinner => WinningTeam > 0;

        public Stack<MoveAction> MoveItems;
        public List<ShotAction> ShotItems;

        public int CurrentRoundNumber;
        public int CurrentStepNumber;
        public List<List<MatchEvent>> EventsByRound;

        public Dictionary<string, List<MatchEventHandler>> EventHandlers;

        public Dictionary<string, List<ExpandoObject>> MatchData;

        // TODO should this be cached on the Match itself, or somewhere else?
        // should there be a MatchService or something that modifies the matches,
        // leaving the Match class to be just the model?
        public StepDto LastStepDto;

        private readonly ICommunicationService _commService;
        private readonly IUserService _userService;

        public Match(string parentLobbyId, Map map, List<Player> players, List<Character> characters,
            GameMode gameMode, ICommunicationService commService, IUserService userService)
        {
            this.ParentLobbyId = parentLobbyId;
            this.Map = map;
            this.Players = players;
            this.Characters = characters;
            this.GameMode = gameMode;
            this.MoveItems = new();
            this.ShotItems = new();
            this.CurrentStepNumber = 1;
            this.CurrentRoundNumber = 0;
            this.EventsByRound = new();
            this.MatchData = new();

            this.EventHandlers = new()
            {
                { INIT, new() },
                { STEP_END, new() },
                { ROUND_END, new() }
            };

            this.LastStepDto = new();

            this._commService = commService;
            this._userService = userService;
        }

        public void InitMap()
        {
            for (int i = 0; i < Map.Tiles.GetLength(0); i++)
            {
                for (int j = 0; j < Map.Tiles.GetLength(1); j++)
                {
                    Map.Tiles[i, j].CreateBehaviour(this, new Coords(i, j));
                }
            }

        }

        public void InitGameMode()
        {
            foreach (var initHandler in GameMode.InitMethods)
            {
                initHandler(this);
            }
        }

        public void InitCharacters()
        {
            foreach (var player in Players)
            {
                player.Character.CreateBehaviour.Invoke(this, player);
            }
        }

        public async void InitAndSendInitialStep()
        {
            this.LastStepDto = new()
            {
                MapString = this.Map.ToString(),
                GameModeInfo = this.GameMode.InfoQuery(this),
                PlayerDtos = this.Players.Select(player => new MatchPlayerDto
                (
                    name: player.UserData.Name,
                    stepCommand: string.Empty,
                    characterIndex: this.Characters.FindIndex(character => character.Name == player.Character.Name),
                    seatIndex: player.SeatIndex,
                    teamIndex: player.TeamIndex,
                    tilesMoved: new List<Coords> { player.Coords },
                    shotMarks: new List<TileMark>()
                )).ToList()
            };

            await this._commService.SendStepData(this.ParentLobbyId, this.LastStepDto);
        }

        public void ExecuteRound()
        {
            EventsByRound.Add(new());

            var commandsForRound = this.GetPlayerInputs(this.Players);

            for (int i = 0; i < 3; i++)
            {
                var commandsForStep = commandsForRound[i];

                ExecuteStep(commandsForStep);

                if (HasWinner)
                {
                    return;
                }
            }

            CurrentRoundNumber++;
            CurrentStepNumber = 1;
        }

        public void ExecuteStep(List<Command> currentStepCommands)
        {
            var stepDto = new StepDto
            {
                MapString = this.Map.ToString(),
                PlayerDtos = this.Players.Select(player =>
                    new MatchPlayerDto {
                        Name = player.UserData.Name,
                        TeamIndex = player.TeamIndex,
                        // TODO there's gotta be a better way of connecting the character in the match to the one in the lobby
                        CharacterIndex = this.Characters.FindIndex(character => character.Name == player.Character.Name),
                        SeatIndex = player.SeatIndex
                    }).ToList()
            };

            ExecuteCommandPhase(currentStepCommands, stepDto);

            while (MoveItems.Count > 0
                || ShotItems.Count > 0)
            {
                HandleMovement(stepDto);

                var markedTiles = HandleShooting(stepDto);

                MarkDamage(markedTiles);
            }

            HandleEndStep(stepDto);

            this.LastStepDto = stepDto;

            this._commService.SendStepData(this.ParentLobbyId, stepDto);
        }

        public void ExecuteCommandPhase(List<Command> currentStepCommands, StepDto stepDto)
        {
            for (int i = 0; i < currentStepCommands.Count; i++)
            {
                var currentCommand = currentStepCommands[i];
                var behaviourResult = currentCommand.Payload.Invoke(this, currentCommand.Direction, Players[i]);

                if (behaviourResult.MoveResult.RemainingRange != 0)
                {
                    MoveItems.Push(behaviourResult.MoveResult);
                }

                foreach (var shotItem in behaviourResult.ShotResults)
                {
                    ShotItems.Add(shotItem);
                }

                stepDto.PlayerDtos[i].StepCommand = currentCommand.ToString();

            }
        }

        public void HandleMovement(StepDto stepDto)
        {
            while (MoveItems.TryPop(out var currentMoveItem))
            {
                var currentPlayer = currentMoveItem.Player;

                var originCoords = currentMoveItem.Coords;
                Console.WriteLine($"Moving player {currentPlayer}");

                var allCoordsForMove = MoveSystem.Consume(this, currentMoveItem);

                var destinationCoords = allCoordsForMove[^1];

                if (destinationCoords.EqualsIgnoringDirection(originCoords))
                {
                    // TODO handle invalid move here
                    Console.WriteLine($"Invalid move!");
                }
                else
                {
                    Console.WriteLine($"Resulting location: {destinationCoords}");
                    currentPlayer.Coords = destinationCoords;

                    var moveEvent = EventSnapshotBuilders.MoveEvent(CurrentStepNumber, currentPlayer, allCoordsForMove);

                    this.HandleEvent(EventType.MoveEvent.ToString(), moveEvent, currentPlayer);

                    var currentPlayerDto = stepDto.PlayerDtos.FirstOrDefault(dto => dto.Name == currentPlayer.UserData.Name);

                    if (currentPlayerDto is not null)
                    {
                        // TODO do we need this deep copy for safety?
                        currentPlayerDto.TilesMoved = allCoordsForMove
                            .Select(coords => new Coords(coords.Row, coords.Col, coords.PartialDirection))
                            .ToList();
                    }
                }

            }
        }

        public List<TileMark> HandleShooting(StepDto stepDto)
        {
            var markedTiles = new List<TileMark>();

            for (int i = 0; i < Players.Count; i++)
            {
                Player currentPlayer = this.Players[i];
                var currentPlayerShots = ShotItems.Where(shotItem => shotItem.Player == currentPlayer).ToList();

                if (currentPlayerShots.Count == 0)
                {
                    continue;
                }

                var originShot = currentPlayerShots.First(shotItem => shotItem.Coords.EqualsIgnoringDirection(currentPlayer.Coords));

                var originShotCoords = new Coords(originShot.Coords.Row, originShot.Coords.Col, originShot.Coords.PartialDirection);
                var originShotDirection = originShot.Direction;

                foreach (var shotItem in currentPlayerShots)
                {
                    markedTiles.AddRange(ShotSystem.Consume(this, shotItem));

                    ShotItems.Remove(shotItem);
                }

                var shootEvent = EventSnapshotBuilders.ShootEvent(CurrentStepNumber, currentPlayer, markedTiles);

                this.HandleEvent(EventType.ShootEvent.ToString(), shootEvent, currentPlayer);

                stepDto.PlayerDtos[i].ShotMarks.AddRange(markedTiles);
            }

            return markedTiles;
        }

        public void MarkDamage(List<TileMark> markedTiles)
        {
            foreach (var currentPlayer in Players)
            {
                var potentialHitsPerTeam = markedTiles
                    .Where(mark => mark.Coords.EqualsIgnoringDirection(currentPlayer.Coords))
                    .GroupBy(mark => mark.OriginPlayer)
                    .ToList();

                var playersThatHit = currentPlayer.Character.MarkBehaviour(this, currentPlayer, potentialHitsPerTeam)
                    .Where(pl => pl.TeamIndex != currentPlayer.TeamIndex);

                foreach (var playerThatHit in playersThatHit)
                {
                    Console.WriteLine($"Player {currentPlayer} hit by {playerThatHit}");

                    var markEvent = EventSnapshotBuilders.MarkEvent(CurrentStepNumber, currentPlayer, playerThatHit);

                    this.HandleEvent(EventType.MarkEvent.ToString(), markEvent, currentPlayer);
                }
            }
        }

        private void HandleEndStep(StepDto stepDto)
        {
            for (int i = EventHandlers[STEP_END].Count - 1; i >= 0; i--)
            {
                var handler = EventHandlers[STEP_END][i].Handler;
                handler.Invoke(this, new MatchEvent(this.CurrentStepNumber, "None", new ExpandoObject()));
            }

            Cleanup(stepDto);

        }

        private void Cleanup(StepDto stepDto)
        {
            foreach (var player in Players)
            {
                player.Coords = player.Coords.ToCenter();
            }

            Console.WriteLine($"Step {CurrentStepNumber} Done!\r\n");

            stepDto.GameModeInfo = this.GameMode.InfoQuery(this);

            var newWinningTeam = this.GameMode.VictoryQuery(this);

            this.WinningTeam = newWinningTeam;

            this.CurrentStepNumber++;
        }

        public void HandleEvent(string eventType, MatchEvent eventSnapshot, Player player)
        {
            if (EventHandlers.ContainsKey(eventType))
            {
                for (int i = EventHandlers[eventType].Count - 1; i >= 0; i--)
                {
                    var eventItem = EventHandlers[eventType][i];
                    if (eventItem.Player is null
                        || eventItem.Player is Player p && p == player)
                    {
                        eventItem.Handler.Invoke(this, eventSnapshot);
                    }
                }

            }

            LogEvent(eventSnapshot);
        }

        private void LogEvent(MatchEvent matchEvent)
        {
            EventsByRound[CurrentRoundNumber].Add(matchEvent);
        }

        public List<MatchEvent> GetCurrentRoundEvents()
        {
            return EventsByRound[CurrentRoundNumber];
        }

        public List<MatchEvent> GetCurrentStepEvents()
        {
            return GetCurrentRoundEvents().Where(e => e.Step == CurrentStepNumber).ToList();
        }

        public void AddSingleFireStepEndHandler(MatchHandlerFunction handlerPayload)
        {
            var stepEndHandlers = EventHandlers[STEP_END];

            void singleFireHandler(Match match, MatchEvent eventData)
            {
                handlerPayload(match, eventData);
                stepEndHandlers.RemoveAll(handlerWrapper => handlerWrapper.Handler == singleFireHandler);
            }

            stepEndHandlers.Add(new(singleFireHandler));
        }

        public void AddStepEndHandler(MatchHandlerFunction handlerPayload)
        {
            var stepEndHandlers = EventHandlers[STEP_END];

            stepEndHandlers.Add(new(handlerPayload));

        }

        public void AddUniqueStepEndHandler(MatchHandlerFunction handlerPayload)
        {
            var stepEndHandlers = EventHandlers[STEP_END];

            if (stepEndHandlers.FirstOrDefault(handlerWrapper => handlerWrapper.Handler == handlerPayload) is null)
            {
                stepEndHandlers.Add(new(handlerPayload));
            }

        }

        public void AddSingleFireHandlerForPlayer(string eventType, MatchHandlerFunction handlerPayload, Player player)
        {
            void singleFireHandler(Match match, MatchEvent eventData)
            {
                handlerPayload(match, eventData);
                EventHandlers[eventType].RemoveAll(item => item.Handler == singleFireHandler);
            }

            this.AddHandlerForPlayer(eventType, singleFireHandler, player);
        }

        public void AddGlobalSingleFireHandler(string eventType, MatchHandlerFunction handlerPayload)
        {
            void singleFireHandler(Match match, MatchEvent eventData)
            {
                handlerPayload(match, eventData);
                EventHandlers[eventType].RemoveAll(item => item.Handler == singleFireHandler);
            }

            this.AddGlobalHandler(eventType, singleFireHandler);
        }

        public void AddGlobalHandler(string eventType, MatchHandlerFunction handler)
        {
            if (EventHandlers.ContainsKey(eventType))
            {
                var alreadyContainsHandler = EventHandlers[eventType].FirstOrDefault(eventItem => eventItem.Handler == handler) is not null;

                if (alreadyContainsHandler)
                {
                    return;
                }
                else
                {
                    EventHandlers[eventType].Add(new MatchEventHandler(handler));
                }
            }
            else
            {
                EventHandlers.Add(eventType, new List<MatchEventHandler>() { new MatchEventHandler(handler) });
            }
        }

        public void AddHandlerForPlayer(string eventType, MatchHandlerFunction handler, Player player)
        {
            if (EventHandlers.ContainsKey(eventType))
            {
                EventHandlers[eventType].Add(new MatchEventHandler(handler, player));
            }
            else
            {
                EventHandlers.Add(eventType, new List<MatchEventHandler>() { new MatchEventHandler(handler, player) });
            }
        }

        public void SetTile(int row, int col, TileBuilder builder)
        {
            var currentCoords = new Coords(row, col);

            Map.Tiles[row, col].RemoveBehaviour(this, currentCoords);

            Map.Tiles[row, col] = builder();

            Map.Tiles[row, col].CreateBehaviour(this, currentCoords);
        }

        public void AddDataItem(string key, ExpandoObject data)
        {
            if (!MatchData.ContainsKey(key))
            {
                MatchData.Add(key, new());
            }

            MatchData[key].Add(data);
        }

        public void UpdateDataItem(string key, ExpandoObject oldData, ExpandoObject newData)
        {
            if (MatchData.ContainsKey(key))
            {
                if (RemoveDataItem(key, oldData))
                {
                    AddDataItem(key, newData);
                }
            }
            else
            {
                AddDataItem(key, newData);
            }
        }

        public void UpdateMatchingDataItem(string key, ExpandoObject newData, Func<dynamic, bool> predicate)
        {
            var targetItem = GetDataItem(key, predicate);

            if (targetItem is not null)
            {
                UpdateDataItem(key, targetItem, newData);
            }
        }

        public ExpandoObject? GetDataItem(string key, Func<dynamic, bool> predicate)
        {
            if (MatchData.ContainsKey(key))
            {
                return MatchData[key].First(predicate);
            }
            else
            {
                return null;
            }

        }

        public bool RemoveDataItem(string key, ExpandoObject targetItem)
        {
            if (MatchData.ContainsKey(key))
            {
                return MatchData[key].Remove(targetItem);
            }
            else
            {
                return false;
            }
        }

        public bool RemoveMatchingDataItem(string key, Func<dynamic, bool> predicate)
        {
            if (MatchData.ContainsKey(key))
            {
                var targetItem = GetDataItem(key, predicate);

                return targetItem switch
                {
                    not null => RemoveDataItem(key, targetItem),
                    _ => false
                };
            }
            else
            {
                return false;
            }
        }

        public int RemoveAllMatchingDataItems(string key, Predicate<dynamic> predicate)
        {
            if (MatchData.ContainsKey(key))
            {
                return MatchData[key].RemoveAll(predicate);
            }

            return 0;
        }

        // TODO what is this supposed to be? implement or remove
        //internal void RemoveMatchingDataItem(object bLINKWALLS, Func<dynamic, bool> p)
        //{
        //    throw new NotImplementedException();
        //}

        private async Task PromptUsersForInput(List<Player> players)
        {
            Task[] inputPromptTasks = new Task[players.Count];

            for (int i = 0; i < players.Count; i++)
            {
                Player currentPlayer = players[i];
                var connectionString = this._userService.GetConnectionIdByUserId(currentPlayer.UserData.Id);

                inputPromptTasks[i] = this._commService.PromptUserForInput(connectionString);
            }

            await Task.WhenAll(inputPromptTasks);
        }

        private List<List<Command>> ParseCommandStrings(List<string> commandStrings)
        {
            var parsedCommandsPerPlayer = new List<List<Command>>();

            for (int i = 0; i < commandStrings.Count; i++)
            {
                parsedCommandsPerPlayer.Add(new());

                if (AreCommandsValid(commandStrings[i]))
                {
                    for (int idx = 0; idx < 3; idx++)
                    {
                        parsedCommandsPerPlayer[i].Add(ParseCommandFromString(commandStrings[i].Substring(2 * idx, 2), this.Players[i]));
                    }
                }
                else
                {
                    for (int idx = 0; idx < 3; idx++)
                    {
                        parsedCommandsPerPlayer[i].Add(CommandBuilders.InvalidMoveCommand(this, Direction.Center, this.Players[i]));
                    }
                }
            }

            var parsedCommandsPerStep = new List<List<Command>>();

            for (int i = 0; i < 3; i++)
            {
                parsedCommandsPerStep.Add(new());

                var currentStepCommands = parsedCommandsPerStep[i];

                for (int idx = 0; idx < this.Players.Count; idx++)
                {
                    Player? player = this.Players[idx];

                    currentStepCommands.Add(parsedCommandsPerPlayer[idx][i]);
                }
            }

            return parsedCommandsPerStep;

        }

        private static bool AreCommandsValid(string commandString)
        {
            return commandString.Count(c => c.Equals('S')) < 2
                && commandString.Count(c => c.Equals('A')) < 2;
        }

        private Command ParseCommandFromString(string commandString, Player player)
        {
            char commandType = commandString[0];
            char direction = commandString[1];

            Direction dir = direction switch
            {
                'U' => Direction.Up,
                'R' => Direction.Right,
                'D' => Direction.Down,
                'L' => Direction.Left,
                _ => Direction.Up,
            };

            return commandType switch
            {
                'M' => CommandBuilders.MoveCommand(this, dir, player),
                'S' => CommandBuilders.ShootCommand(this, dir, player),
                'A' => CommandBuilders.AbilityCommand(this, dir, player),
                _ => CommandBuilders.InvalidMoveCommand(this, dir, player)
            };
        }
    }
}
