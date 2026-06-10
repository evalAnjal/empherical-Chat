using Microsoft.AspNetCore.SignalR;
using EmphericalChat.Backend.Services;
using EmphericalChat.Backend.Models;

namespace EmphericalChat.Backend.Hubs;

public class ChatHub : Hub
{
    private readonly RoomManager _roomManager;

    public ChatHub(RoomManager roomManager)
    {
        _roomManager = roomManager;
    }

    public async Task JoinRoom(string roomId, string username)
    {
        var room = _roomManager.GetRoom(roomId);
        if (room == null)
        {
            await Clients.Caller.SendAsync("Error", "Room has expired or does not exist.");
            Context.Abort();
            return;
        }

        lock (room.ConnectionIds)
        {
            room.ConnectionIds.Add(Context.ConnectionId);
        }

        await Groups.AddToGroupAsync(Context.ConnectionId, roomId);
        await Clients.Group(roomId).SendAsync("ReceiveMessage", new ChatMessage("System", $"{username} joined the room.", DateTime.UtcNow));
    }

    public async Task SendMessage(string roomId, string username, string content)
    {
        var room = _roomManager.GetRoom(roomId);
        if (room == null)
        {
            await Clients.Caller.SendAsync("Error", "Room no longer exists.");
            return;
        }

        var message = new ChatMessage(username, content, DateTime.UtcNow);
        await Clients.Group(roomId).SendAsync("ReceiveMessage", message);
    }
}