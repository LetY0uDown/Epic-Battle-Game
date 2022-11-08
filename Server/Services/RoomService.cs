using Microsoft.EntityFrameworkCore;
using Models.Game;
using Server.Tools;
using System.Text;

namespace Server.Services;

public class RoomService
{
    private static readonly Dictionary<Room, List<Character>> _charactersInRooms = new();

    private readonly DatabaseContext _dbContext;

    public RoomService (DatabaseContext databaseContext)
    {
        _dbContext = databaseContext;
    }

    public async Task<Room> GetRoom (int x, int y)
    {
        var room = await _dbContext.Rooms.FirstOrDefaultAsync(r => r.X == x && r.Y == y);

        if (room is null) {
            room = new RoomBuilder(x, y).WithFurniture()
                                        .WithDecorations()
                                        .WithMoney(0, 25)
                                        .Build();

            try {
                await _dbContext.Rooms.AddAsync(room);
                await _dbContext.SaveChangesAsync();
            }
            catch {
                throw;
            }
        }

        return room;
    }

    public string ConstructRoomDescription (Room room, string callerName)
    {
        var sb = new StringBuilder("Вы вошли в очередную комнату. ").Append(room.Description);

        if (!IsCharacterAlone(room)) {
            sb.Append(" Помимо вас в этой комнате находится ").Append(_charactersInRooms[room].Count).Append(" человек. ");

            sb.Append("Голос в голове твердит вам их имена - ");

            foreach (var character in _charactersInRooms[room]) {
                if (character.Name != callerName)
                    sb.Append(character.Name).Append(' ');
            }
        }

        var roomStatus = room.CurrentStatus switch {
            Room.Status.Peace => "Здесь сейчас относительно тихо",
            Room.Status.Battle => "Прямо сейчас здесь происходит напряжённый бой! Будьте аккуратнее",
            _ => "Тут происходит Swashbuckle"
        };

        sb.AppendLine(roomStatus);

        return sb.ToString();
    }

    public bool IsCharacterAlone(Room room)
    {
        return _charactersInRooms[room].Count == 1;
    }

    public void RemoveCharacterFromRoom(Character character, Room room)
    {
        if (!_charactersInRooms.ContainsKey(room))
            return;

        _charactersInRooms[room].Remove(character);
    }

    public void AddCharacterToRoom (Character character, Room room)
    {
        if (!_charactersInRooms.ContainsKey(room)) {
            _charactersInRooms.Add(room, new List<Character>());
        }

        _charactersInRooms[room].Add(character);
    }
}