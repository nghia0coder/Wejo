using Microsoft.AspNetCore.SignalR;

namespace Wejo.Realtime.API.Hubs;

using Common.Core.Requests;

public sealed class NotificationHub : Hub
{
    #region -- Overrides --

    /// <summary>
    /// Called when a new connection is established with the hub.
    /// </summary>
    /// <returns>A <see cref="Task"/> that represents the asynchronous connect.</returns>
    public override async Task OnConnectedAsync()
    {
        var req = new BaseR(Context.GetHttpContext());

        var userId = req.UserId;
        if (userId != null)
        {
            await JoinGroup(userId);
        }

        await Clients.All.SendAsync("onConnected", $"ClientID: {Context.ConnectionId}");
    }

    /// <summary>
    /// Called when a connection with the hub is terminated.
    /// </summary>
    /// <returns>A <see cref="Task"/> that represents the asynchronous disconnect.</returns>
    public override Task OnDisconnectedAsync(Exception? exception)
    {
        return base.OnDisconnectedAsync(exception);
    }

    #endregion

    #region -- Methods --

    /// <summary>
    /// Initialize
    /// </summary>
    public NotificationHub() { }

    public async Task JoinGroup(string group)
    {
        Console.WriteLine($"Client {Context.ConnectionId} joined group: {group}");
        await Groups.AddToGroupAsync(Context.ConnectionId, group);
    }

    #endregion
}
