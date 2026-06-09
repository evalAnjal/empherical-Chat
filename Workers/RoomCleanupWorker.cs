using EmphericalChat.Backend.Services;

namespace EphemeralChat.Backend.Workers;

public class RoomCleanupWorker : BackgroundService
{
    private readonly RoomManager _roomManager;
    private readonly PeriodicTimer _timer = new(TimeSpan.FromSeconds(30));

    public RoomCleanupWorker(RoomManager roomManager)
    {
        _roomManager = roomManager;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested && await _timer.WaitForNextTickAsync(stoppingToken))
        {
            _roomManager.RemoveExpiredRooms();
        }
    }
}