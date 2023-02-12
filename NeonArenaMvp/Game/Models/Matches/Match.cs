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

namespace NeonArenaMvp.Game.Models.Matches
{
    public class Match
    {
        public Lobby Lobby;
        public Map Map;
        public List<Player> Players;
        public int PlayerCount;
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

        public Match(Lobby lobby, Map map, List<Player> players, GameMode gameMode)
        {
            Lobby = lobby;
            Map = map;
            Players = players;
            PlayerCount = players.Count;
            GameMode = gameMode;
            MoveItems = new();
            ShotItems = new();
            CurrentStepNumber = 1;
            CurrentRoundNumber = 0;
            EventsByRound = new();
            MatchData = new();

            EventHandlers = new()
            {
                { INIT, new() },
                { STEP_END, new() },
                { ROUND_END, new() }
            };

            InitMap();
            InitGameMode();
            InitCharacters();

            this.LastStepDto = new()
            {
                MapString = this.Map.ToString(),
                GameModeInfo = this.GameMode.InfoQuery(this),
                PlayerDtos = this.Players.Select(player => new MatchPlayerDto
                (
                    name: player.Name,
                    stepCommand: string.Empty,
                    characterIndex: this.Lobby.Characters.FindIndex(character => character.Name == player.Character.Name),
                    seatIndex: this.Lobby.Seats.Keys.ToList().IndexOf(player.Color),
                    teamIndex: player.Team,
                    tilesMoved: new List<Coords> { player.Coords },
                    shotMarks: new List<TileMark>()
                )).ToList()
            };
        }

        private void InitMap()
        {
            for (int i = 0; i < Map.Tiles.GetLength(0); i++)
            {
                for (int j = 0; j < Map.Tiles.GetLength(1); j++)
                {
                    Map.Tiles[i, j].CreateBehaviour(this, new Coords(i, j));
                }
            }

        }

        private void InitGameMode()
        {
            foreach (var initHandler in GameMode.InitMethods)
            {
                initHandler(this);
            }
        }

        private void InitCharacters()
        {
            foreach (var player in Players)
            {
                player.Character.CreateBehaviour.Invoke(this, player);
            }
        }

        public void ExecuteRound(List<List<Command>> commandsForRound)
        {
            EventsByRound.Add(new());

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
                        Name = player.Name,
                        TeamIndex = player.Team,
                        // TODO there's gotta be a better way of connecting the character in the match to the one in the lobby
                        CharacterIndex = this.Lobby.Characters.FindIndex(character => character.Name == player.Character.Name),
                        SeatIndex = this.Lobby.Seats.Keys.ToList().IndexOf(player.Color)
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

            Lobby.SendLatestStep();
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

                    var currentPlayerDto = stepDto.PlayerDtos.FirstOrDefault(dto => dto.Name == currentPlayer.Name);

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
                    .Where(pl => pl.Team != currentPlayer.Team);

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
    }
}
