namespace NeonArenaMvp.Network.SignalR
{
    using Microsoft.AspNetCore.SignalR;
    using NeonArenaMvp.Network.Services.Interfaces;
    using System.Threading.Tasks;

    public class GameHub
        : Hub<IGameHubClient>
    {
        private readonly IHubContext<GameHub, IGameHubClient> hubContext;
        private readonly ILobbyService lobbyService;
        private readonly IUserService userService;
        private readonly ICommService commService;

        public GameHub(IHubContext<GameHub, IGameHubClient> hubContext, ILobbyService lobbyService, IUserService userService, ICommService commService)
        {
            this.hubContext = hubContext;
            this.lobbyService = lobbyService;
            this.userService = userService;
            this.commService = commService;
        }

        public override Task OnConnectedAsync()
        {
            var (userName, existingUserId) = this.ParseHeaders();

            if (userName is not null)
            {
                var user = userService.GetOrCreateUser(
                    name: userName,
                    existingUserId: existingUserId,
                    connectionId: this.Context.ConnectionId);

                this.commService.AddUserConnection(user.Id, this.Context.ConnectionId);

                Console.WriteLine($"User with name {user.Name} connected with ID:{this.Context.ConnectionId}");
            }

            return base.OnConnectedAsync();
        }

        public async Task CreateLobby()
        {
            var (userName, existingUserId) = this.ParseHeaders();

            if (userName is null)
            {
                return;
            }

            var user = this.userService.GetOrCreateUser(userName, existingUserId, this.Context.ConnectionId);

            await this.lobbyService.Create(user);

            Clients.All.ReceiveLobbyList();
        }

        public async Task JoinLobby(Guid lobbyId)
        {
            var (userName, existingUserId) = this.ParseHeaders();

            if (userName is not null)
            {
                var user = userService.GetOrCreateUser(
                    name: userName,
                    existingUserId: existingUserId,
                    connectionId: this.Context.ConnectionId);

                await this.lobbyService.JoinLobby(lobbyId, user);
            }
        }

        private (string? userName, Guid? existingUserId) ParseHeaders()
        {
            var requestHeaders = this.Context.GetHttpContext()?.Request.Headers;

            var userName = requestHeaders?["User-Name"].Single();
            var hasExistingId = Guid.TryParse(requestHeaders?["User-Id"].SingleOrDefault(), out var existingId);

            return (userName, hasExistingId ? existingId : null);
        }
    }
}
