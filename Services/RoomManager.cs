using System.Collections.Concurrent;
using EmphericalChat.Backend.Models;

namespace EmphericalChat.Backend.Services;

public class RoomManager
{
    private readonly ConcurrentDictionary<string, ChatRoom> _rooms = new();
    private readonly TimeSpan _roomLifeTime = TimeSpan.FromMinutes(10);

    public string CreateRoom()
    {
        var roomId = Guid.NewGuid().ToString("N")[..8];
        var room = new ChatRoom(roomId,DateTime.UtcNow);
        _rooms.TryAdd(roomId,room);
        return roomId;
    }

    public ChatRoom? GetRoom(string roomId)
    {
        return _rooms.TryGetValue(roomId, out var room ) ? room : null;
    }

    public void RemoveExpiredRooms()
    {
        var cutoff = DateTime.UtcNow - _roomLifeTime;
        foreach (var (roomId,room) in _rooms)
        {
            if (room.CreatedAt < cutoff)
            {
                _rooms.TryRemove(roomId,out _);
            }
        }
    }
}