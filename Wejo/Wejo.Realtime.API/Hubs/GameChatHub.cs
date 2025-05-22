using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace Wejo.Realtime.API.Hubs;

using Common.Core.Constants;
using Common.Domain.Dtos;

[Authorize]
public sealed class GameChatHub : Hub
{
    #region -- Overrides --

    /// <summary>
    /// Called when a connection with the hub is terminated.
    /// </summary>
    /// <returns>A <see cref="Task"/> that represents the asynchronous disconnect.</returns>
    public override Task OnDisconnectedAsync(Exception? exception)
    {
        return base.OnDisconnectedAsync(exception);
    }

    #endregion
    public async Task JoinGameChat(string gameId)
    {
        Console.WriteLine($"Client {Context.ConnectionId} joined group: {gameId}");
        await Groups.AddToGroupAsync(Context.ConnectionId, gameId);
    }

    public async Task LeaveGameChat(string gameId)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, gameId);
    }

    public async Task SendGameMessage(GameChatMessageDto message)
    {
        await Clients.Group(message.GameId.ToString()).SendAsync(RealTimeTopic.ReceiveGameMessage, message);
    }
}