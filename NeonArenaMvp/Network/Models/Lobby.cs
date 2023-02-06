using NeonArenaMvp.Game.Helpers.Builders;
using NeonArenaMvp.Game.Models.Maps;
using NeonArenaMvp.Game.Models.Matches;
using NeonArenaMvp.Game.Models.Players;
using NeonArenaMvp.Network.Models.Dto.Lobby;
using NeonArenaMvp.Network.Models.Dto.Step;
using NeonArenaMvp.Network.Services.Interfaces;

using Newtonsoft.Json;

using static NeonArenaMvp.Game.Helpers.Models.Directions;
using static NeonArenaMvp.Network.Helpers.Constants;

namespace NeonArenaMvp.Network.Models
{
    public class Lobby
    {
        public readonly Guid Id;
        public LobbyState State;
        public User Host;
        public List<User> Users;

        public Dictionary<Color, User?> Seats;
        public Dictionary<string, int> UserIdToCharacterId;
        public Dictionary<string, int> UserIdToTeamId;

        // TODO temporary hard-coded workaround for the MVP
        public List<Character> Characters;

        public List<Map> Maps;
        public List<GameMode> GameModes;
        public Map? CurrentMap => this.Maps.ElementAtOrDefault(0);
        public GameMode? CurrentGameMode => this.GameModes.ElementAtOrDefault(0);
        public List<User?> Spectators => this.Users.Except(this.Seats.Values).ToList();

        public int CurrentMapIndex;
        public int CurrentGameModeIndex;
        public Match? ActiveMatch;

        private readonly ICommunicationService _commService;

        public Lobby(User host, Guid lobbyId, ICommunicationService commService)
        {
            this.Id = lobbyId;
            this.State = LobbyState.Open;
            this.Host = host;

            this._commService = commService;

            this.Users = new();

            this.AddUser(host);

            this.Seats = new();
            foreach (var color in Enum.GetValues<Color>()[..^1])
            {
                this.Seats.Add(color, null);
            }

            this.Characters = new()
            {
                CharacterBuilders.Maclaurin,
                CharacterBuilders.Nova,
                CharacterBuilders.Antonio,
                CharacterBuilders.Gloria,
                CharacterBuilders.Ryker,
                CharacterBuilders.Svarog,
                CharacterBuilders.Val
            };

            this.UserIdToCharacterId = new();
            this.UserIdToTeamId = new();

            // TODO add methods for adding/removing maps and gamemodes
            this.Maps = new();
            this.GameModes = new();
            this.CurrentMapIndex = 0;
            this.CurrentGameModeIndex = 0;
            this.ActiveMatch = null;
        }

        public void AddUser(User user)
        {
            if (this.Users.Contains(user))
            {
                return;
            }

            this.Users.Add(user);
        }

        public void RemoveUser(User user)
        {
            var targetUserSeatColor = Color.White;
            foreach (var color in this.Seats.Keys)
            {
                if (this.Seats[color]?.Id == user.Id)
                {
                    targetUserSeatColor = color;
                }
            }

            if (targetUserSeatColor != Color.White)
            {
                UnassignSeat(targetUserSeatColor);
            }

            this.Users.RemoveAll(u => u.Id == user.Id);

            // TODO in the lobby service, after calling this method, check if the lobby
            // has no players left in it, and remove it from the lobby list if that's the case
        }

        public void AssignUserToSeat(string userId, int seatIndex)
        {
            var seatColor = (Color)seatIndex;

            // if the user is already seated in another seat
            var existingSeatForUser = this.Seats.FirstOrDefault(kvp => kvp.Value?.Id == userId);
            if (existingSeatForUser.Value is not null)
            {
                // first unassign the user from the existing seat
                // TODO NOW: figure out a way to do a 'reassign/swap' instead of unassigning,
                // since it removes the user's cached team/character index
                this.UnassignSeat(existingSeatForUser.Key);
            }

            var targetUser = this.Users.FirstOrDefault(user => user.Id == userId);

            if (targetUser is null)
            {
                throw new Exception("The user you're trying to unassign doesn't exist in this lobby!");
            }

            this.Seats[seatColor] = targetUser;
            this.UserIdToCharacterId.Add(targetUser.Id, 0);
            this.UserIdToTeamId.Add(targetUser.Id, this.GetTeamIndexForUser(targetUser));
        }

        public void UnassignSeat(Color seatColor)
        {
            this.Seats.TryGetValue(seatColor, out var targetUser);

            if (targetUser is null)
            {
                throw new Exception("The user you're trying to unassign doesn't exist in this lobby!");
            }

            this.UserIdToCharacterId.Remove(targetUser.Id);
            this.UserIdToTeamId.Remove(targetUser.Id);
            this.Seats[seatColor] = null;
        }

        public void SetCharacterForUser(string userId, int characterIndex)
        {
            if (characterIndex > (this.Characters.Count - 1))
            {
                // TODO this should either throw an exception or 
                // just an error message to the client (depends on how we handle errors in communication)
                return;
            }

            this.UserIdToCharacterId[userId] = characterIndex;
        }

        public void SetTeamForUser(string userId, int teamIndex)
        {
            // TODO do we need some kind of index validation for teams?
            // maybe as a way to keep the max team value tidy?
            // or is that already achieved by how the client and server work together?
            this.UserIdToTeamId[userId] = teamIndex;
        }

        public void RunMatch()
        {
            if (this.CurrentMap is not null
                && this.CurrentGameMode is not null)
            {
                var currentlySeatedUsers = this.Seats.Where(kvp => kvp.Value is not null).ToList();

                if (currentlySeatedUsers.Count < 1)
                {
                    // TODO send an actual message to the client(s) and return rather than throwing
                    throw new Exception("Can't start a game with no players!");
                }

                List<User?> usersToTake = currentlySeatedUsers.Select(kvp => kvp.Value).ToList();

                if (currentlySeatedUsers.Count > this.CurrentMap.StartingPositions.Count)
                {
                    usersToTake = currentlySeatedUsers.Take(this.CurrentMap.StartingPositions.Count).Select(kvp => kvp.Value).ToList();
                }

                var userIdToColorReverseMap = usersToTake.ToDictionary(user => user.Id, user => this.Seats.First(seat => seat.Value?.Id == user?.Id).Key);

                List<Player> mapPlayers = new();

                for (int i = 0; i < usersToTake.Count; i++)
                {
                    var currentUser = usersToTake[i];

                    mapPlayers.Add(new Player
                    (
                        color: userIdToColorReverseMap[currentUser.Id],
                        name: currentUser.Name,
                        coords: this.CurrentMap.StartingPositions[i],
                        // TODO call into some service that would manage the currently loaded characters
                        // or keep the currently loaded characters in the lobby itself?
                        // the latter implies a generic solution for mods, which is out of MVP scope
                        character: this.Characters[this.UserIdToCharacterId[currentUser.Id]],
                        team: this.UserIdToTeamId[currentUser.Id]
                    ));
                }

                this.ActiveMatch = new(this, this.CurrentMap, mapPlayers, this.CurrentGameMode);

                while (!this.ActiveMatch.HasWinner)
                {
                    var playerInputs = this.GetPlayerInputs();

                    var playerCommands = this.ParseCommandStrings(playerInputs);

                    this.ActiveMatch.ExecuteRound(playerCommands);
                }

                Console.WriteLine($"Team {this.ActiveMatch.WinningTeam} wins!");
            }
        }

        // TODO rework to accept a list of players to take input from,
        // and take the input from the network
        public List<string> GetPlayerInputs()
        {
            var commandList = new List<string>();

            foreach (var player in this.ActiveMatch.Players)
            {
                Console.WriteLine($"Input command string for player {player}:");
                commandList.Add(Console.ReadLine());
            }

            return commandList;
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
                        parsedCommandsPerPlayer[i].Add(ParseCommandFromString(commandStrings[i].Substring(2 * idx, 2), this.ActiveMatch.Players[i]));
                    }
                }
                else
                {
                    for (int idx = 0; idx < 3; idx++)
                    {
                        parsedCommandsPerPlayer[i].Add(CommandBuilders.InvalidMoveCommand(this.ActiveMatch, Direction.Center, this.ActiveMatch.Players[i]));
                    }
                }
            }

            var parsedCommandsPerStep = new List<List<Command>>();

            for (int i = 0; i < 3; i++)
            {
                parsedCommandsPerStep.Add(new());

                var currentStepCommands = parsedCommandsPerStep[i];

                for (int idx = 0; idx < Users.Count; idx++)
                {
                    Player? player = this.ActiveMatch.Players[idx];

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
                'M' => CommandBuilders.MoveCommand(this.ActiveMatch, dir, player),
                'S' => CommandBuilders.ShootCommand(this.ActiveMatch, dir, player),
                'A' => CommandBuilders.AbilityCommand(this.ActiveMatch, dir, player),
                _ => CommandBuilders.InvalidMoveCommand(this.ActiveMatch, dir, player)
            };
        }

        public void SendStep(StepDto stepDto)
        {
            using (StreamWriter writer = new(@".\RoundOutput.txt", true))
            {
                writer.WriteLine(JsonConvert.SerializeObject(stepDto));
            }
        }

        public LobbyDto ToDto()
        {
            var lobbyDto = new LobbyDto
            (
                id: this.Id.ToString(),
                hostName: this.Host.Name,
                characters: this.Characters.Select(character => character.Name).ToList(),
                users: this.MapUsersToDtos()
            );

            return lobbyDto;
        }

        private List<LobbyUserDto> MapUsersToDtos()
        {
            return this.Users.Select(user =>
            {
                return new LobbyUserDto
                (
                    name: user.Name,
                    selectedSeatIndex: this.GetSeatIndexForUser(user),
                    selectedTeamIndex: this.GetTeamIndexForUser(user),
                    selectedCharacterIndex: this.GetCharacterIndexForUser(user)
                );
            })
            .ToList();
        }

        private int? GetSeatIndexForUser(User user)
        {
            for (int i = 0; i < this.Seats.Count; i++)
            {
                if (this.Seats.ElementAt(i).Value == user)
                {
                    return i;
                }
            }

            return null;
        }

        private int GetCharacterIndexForUser(User user)
        {
            if (this.UserIdToCharacterId.ContainsKey(user.Id))
            {
                return this.UserIdToCharacterId[user.Id];
            }

            return 0;

        }

        private int GetTeamIndexForUser(User user)
        {
            if (this.UserIdToTeamId.ContainsKey(user.Id))
            {
                return this.UserIdToTeamId[user.Id];
            }

            if (this.UserIdToTeamId.Count == 0)
            {
                return 1;
            }
            else
            {
                return this.UserIdToTeamId.Values.Max() + 1;
            }

        }

    }
}
