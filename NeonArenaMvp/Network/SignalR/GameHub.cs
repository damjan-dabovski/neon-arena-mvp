﻿using Microsoft.AspNetCore.SignalR;
using NeonArenaMvp.Network.Services.Interfaces;

namespace NeonArenaMvp.Network.SignalR
{
    public class GameHub : Hub<IGameHubClient>
    {
        private readonly IHubContext<GameHub, IGameHubClient> _hubContext;
        private readonly IUserService _userService;
        private readonly ILobbyService _lobbyService;

        public GameHub(IHubContext<GameHub, IGameHubClient> hubContext, ILobbyService lobbyService, IUserService userService)
        {
            this._hubContext = hubContext;
            this._lobbyService = lobbyService;
            this._userService = userService;
        }

        public override Task OnConnectedAsync()
        {
            var existingUserId = this.Context.GetHttpContext()?.Request.Cookies.FirstOrDefault(cookie => cookie.Key.Equals("playerId")).Value;

            if (existingUserId is not null)
            {
                this._userService.ConnectExistingUser(this.Context.ConnectionId, existingUserId);
            }
            else
            {
                this._userService.AddUser(this.Context.ConnectionId, "TestUser");
            }

            this.Clients.Caller.ReceiveLobbyList(this._lobbyService.GetLobbies());

            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception? exception)
        {
            // TODO determine what logic would need to be executed
            return base.OnDisconnectedAsync(exception);
        }

        public async Task Broadcast(string message)
        {
            await this._hubContext.Clients.All.ReceiveMessage(message);
        }

        public async Task CreateLobby(string hostId)
        {
            await this._lobbyService.CreateLobby(hostId);
        }

        public async Task RemoveLobby(string lobbyId)
        {
            await this._lobbyService.RemoveLobby(lobbyId);
        }

        public async Task JoinLobby(string userId, string lobbyId)
        {
            await this._lobbyService.AddUserToLobby(userId, lobbyId);
        }

        public async Task LeaveLobby(string userId, string lobbyId)
        {
            await this._lobbyService.RemoveUserFromLobby(userId, lobbyId);
        }

        public void RunMatchInLobby(string lobbyId)
        {
            this._lobbyService.RunMatch(lobbyId);
        }

        public void SendInputToLobby(string lobbyId, string userId, string input)
        {
            this._lobbyService.PassUserInputToLobby(lobbyId, userId, input);
        }
    }
}
