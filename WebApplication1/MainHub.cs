using Microsoft.AspNetCore.SignalR;

namespace WebApplication1;

public class MainHub: Hub
{
    public async Task SendMessage(string message)
    {
        await Clients.All.SendAsync("ReceiveMessage", message);
    }
}
