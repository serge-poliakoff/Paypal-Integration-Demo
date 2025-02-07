using Microsoft.AspNetCore.SignalR;

namespace SignalRDemoBlazorApp.Hubs;

public class MainHub : Hub
{
    public override async Task OnConnectedAsync()
    {
        await SendMessage("admin", $"User {Context.ConnectionId} has joined the room");
        await base.OnConnectedAsync();
    }
    public async Task SendMessage(string user, string message)
    {
        await Clients.All.SendAsync("ReceiveMessage", user, message);
    }
}
