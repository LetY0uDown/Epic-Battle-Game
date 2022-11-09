using Models.Game;

namespace Server.Services;

public sealed class BattleService
{
    private static readonly List<Room> _roomsWithBattles = new List<Room>();

    private readonly DatabaseContext _dbContext;

    public BattleService (DatabaseContext databaseContext)
    {
        _dbContext = databaseContext;
    }

    public async void StartBattleInRoom (Room room)
    {
        room.CurrentStatus = Room.Status.Battle;

        _roomsWithBattles.Add(room);

        try {
            _dbContext.Rooms.Update(room);
            await _dbContext.SaveChangesAsync();
        }
        catch {
            throw;
        }
    }
}