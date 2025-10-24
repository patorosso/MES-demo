namespace backend.Hubs;

using Microsoft.AspNetCore.SignalR;

public class LinesHub : Hub
{
    public async Task SendUpdate(string linea, int cantidad)
    {
        await Clients.All.SendAsync("ReceiveUpdate", linea, cantidad);
    }
}

