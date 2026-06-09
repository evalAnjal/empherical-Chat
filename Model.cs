namespace EmphericalChat.Backend.Models;

public record ChatRoom (string RoomId, DateTime CreatedAt)
{
    public HashSet<string> ConnectionIds {get; init;} = new();

}

public record ChatMessage (string User, string Content , DateTime TimeStamp);