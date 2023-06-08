using NeonArenaMvp.Game.Behaviours.GameModes;
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
        public Map? CurrentMap => this.Maps.ElementAtOrDefault(CurrentMapIndex);
        public List<User?> Spectators => this.Users.Except(this.Seats.Values).ToList();

        public int CurrentMapIndex;
        public int CurrentGameModeIndex;
        public Match? ActiveMatch;

        private readonly ICommunicationService _commService;
        private readonly IUserService _userService;

        public Lobby(User host, Guid lobbyId, ICommunicationService commService, IUserService userService)
        {
            this.Id = lobbyId;
            this.State = LobbyState.Open;
            this.Host = host;

            this._commService = commService;
            this._userService= userService;

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


            var deathmatch = new GameMode
            (
                name: "Deathmatch",
                initMethods: new List<Action<Match>>
                {
                    Deathmatch.DeathmatchInit
                },
                winQuery: Deathmatch.DeathmatchWinQuery,
                infoQuery: Deathmatch.DeathmatchInfo
            );

            // TODO add methods for adding/removing maps and gamemodes
            // TODO hardcoded workarounds for MVP
            var tempMap = new Map
            (
                rowSize: 5,
                colSize: 5,
                startingPositions: new() { new Coords(1, 1), new Coords(2, 2), new Coords(3, 3) },
                gameMode: deathmatch
            ).FillEmpty();
            this.Maps = new() { tempMap };

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
                if (this.Seats[color] is User usr
                    && usr.Equals(user))
                {
                    targetUserSeatColor = color;
                }
            }

            var isUserSeated = targetUserSeatColor != Color.White;

            if (isUserSeated)
            {
                UnassignSeat(targetUserSeatColor);
            }

            this.Users.RemoveAll(u => u.Id == user.Id);

            // TODO in the lobby service, after calling this method, check if the lobby
            // has no players left in it, and remove it from the lobby list if that's the case
        }

        public void AssignUserToSeat(string userId, int seatIndex)
        {
            var targetSeatColor = (Color)seatIndex;

            // if the user is already seated in another seat
            var existingSeatForUser = this.Seats.FirstOrDefault(kvp => kvp.Value?.Id == userId);
            if (existingSeatForUser.Value is not null)
            {
                this.SwapSeats(existingSeatForUser.Key, targetSeatColor);
                return;
            }

            var targetUser = this.Users.FirstOrDefault(user => user.Id == userId);

            if (targetUser is null)
            {
                throw new Exception("The user you're trying to assign doesn't exist in this lobby!");
            }

            this.Seats[targetSeatColor] = targetUser;
            this.UserIdToCharacterId.Add(targetUser.Id, 0);
            this.UserIdToTeamId.Add(targetUser.Id, this.GetTeamIndexForUser(targetUser));
        }

        public void SwapSeats(Color sourceSeatColor, Color destinationSeatColor)
        {
            User? temp = this.Seats[sourceSeatColor];

            this.Seats[sourceSeatColor] = this.Seats[destinationSeatColor];
            this.Seats[destinationSeatColor] = temp;
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
            if (this.CurrentMap is not null)
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

                List<Player> matchPlayers = new();

                for (int i = 0; i < usersToTake.Count; i++)
                {
                    var currentUser = usersToTake[i];

                    matchPlayers.Add(new Player
                    (
                        color: userIdToColorReverseMap[currentUser.Id],
                        userData: currentUser,
                        seatIndex: this.Seats.Values.ToList().IndexOf(currentUser),
                        coords: this.CurrentMap.StartingPositions[i],
                        // TODO call into some service that would manage the currently loaded characters
                        // or keep the currently loaded characters in the lobby itself?
                        // the latter implies a generic solution for mods, which is out of MVP scope
                        character: this.Characters[this.UserIdToCharacterId[currentUser.Id]],
                        teamIndex: this.UserIdToTeamId[currentUser.Id]
                    ));
                }

                this.ActiveMatch = new
                (
                    parentLobbyId: this.Id.ToString(),
                    map: this.CurrentMap,
                    players: matchPlayers,
                    characters: this.Characters,
                    gameMode: this.CurrentMap.GameMode,
                    commService: this._commService,
                    userService: this._userService
                );

                this.ActiveMatch.InitMap();
                this.ActiveMatch.InitGameMode();
                this.ActiveMatch.InitCharacters();
                this.ActiveMatch.InitAndSendInitialStep();

                this.SendLatestStep();

                while (!this.ActiveMatch.HasWinner)
                {
                    this.ActiveMatch.ExecuteRound();
                }

                Console.WriteLine($"Team {this.ActiveMatch.WinningTeam} wins!");
            }
        }

        // TODO should this be a generic 'SendStep' method that sends any Step DTO?
        // If there is a MatchService, should it be responsible for either calling this or
        // just providing the right/latest DTO?
        public void SendLatestStep()
        {
            this._commService.SendStepData(this.Id.ToString(), this.ActiveMatch.LastStepDto);
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
