using Microsoft.AspNetCore.SignalR;
using Models.Game;
using Server.Services;
using System.Text;

namespace Server.Controllers;

public class GameHub : Hub
{
    private const string RECIEVE_MSG = "RecieveMsg";

    private readonly DatabaseContext _dbContext;
    private readonly RoomService _roomService;
    private readonly BattleService _battleService;

    public GameHub (DatabaseContext dbContext, RoomService roomService, BattleService battleService)
    {
        _dbContext = dbContext;
        _roomService = roomService;
        _battleService = battleService;
    }

    public async Task MakeAction (Character sender, BattleAction.Type action)
    {

    }

    public async Task EnterRoom(Room currentRoom, Character character, int x, int y)
    {
        await RemovePlayerFromRoom(character, currentRoom);

        await Clients.OthersInGroup(currentRoom.ToString()).SendAsync(RECIEVE_MSG, $"{character.Name} вышел в одну из дверей, и был таков.");

        var newRoom = await _roomService.GetRoom(x, y);

        await AddPlayerToRoom(character, newRoom);

        var roomDesc = _roomService.ConstructRoomDescription(newRoom, character.Name!);

        await Clients.Caller.SendAsync(RECIEVE_MSG, roomDesc);

        await Clients.OthersInGroup(newRoom.ToString()).SendAsync(RECIEVE_MSG, $"Вы видите как в комнату входит {character.Name}. " +
                                                                               $"На нём надет {character.Armor!.Title}, а в руках он держит {character.Weapon!.Title}");
    }

    public async Task Spawn (Character character)
    {
        var room = await _roomService.GetRoom(character.CurrentX, character.CurrentY);

        await AddPlayerToRoom(character, room);

        var roomDesc = _roomService.ConstructRoomDescription(room, character.Name!);

        await Clients.Caller.SendAsync("EnterRoom", roomDesc, room);

        var sb = new StringBuilder("Неожиданно, прямо посреди комнаты появилась какая - то непонятная фигура, на которой надет ").Append(character.Armor!.Title)
                 .Append(", а рядом лежит ").Append(character.Weapon!.Title).Append(". ");

        await Clients.Caller.SendAsync(room.ToString(), RECIEVE_MSG, sb.ToString());

        sb.Append("Что-то подсказывает вам что эту фигуру зовут ").Append(character.Name);

        await Clients.OthersInGroup(room.ToString()).SendAsync(RECIEVE_MSG, sb.ToString());
    }

    public async Task PickUpLoot (Character character, Room room)
    {
        var moneyFound = room.PickMoney();

        try {
            room.MoneyIn -= moneyFound;

            _dbContext.Rooms.Update(room);
            await _dbContext.SaveChangesAsync();
        }
        catch {
            throw;
        }

        await Clients.Caller.SendAsync("FoundMoney", moneyFound);

        await Clients.Group(room.ToString()).SendAsync(RECIEVE_MSG, $"{character.Name} только что нагнулся, и вытащил из какой-то щели несколько монет");
    }

    private async Task RemovePlayerFromRoom (Character character, Room room)
    {
        _roomService.RemoveCharacterFromRoom(character, room);

        await Groups.RemoveFromGroupAsync(Context.ConnectionId, room.ToString());
    }

    private async Task AddPlayerToRoom (Character character, Room room)
    {
        _roomService.AddCharacterToRoom(character, room);

        await Groups.AddToGroupAsync(Context.ConnectionId, room.ToString());
    }
}