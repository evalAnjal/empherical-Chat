using Microsoft.AspNetCore.SignalR;
using EmphericalChat.Backend.Services;
using EmphericalChat.Backend.Models;

namespace EmphericalChat.Backend.Hubs;

public class ChatHub : Hub
{
    private readonly RoomManager _roomManager;

    public ChatHub (RoomManager roomManager)
    {
        _roomManager = roomManager;
    }
}