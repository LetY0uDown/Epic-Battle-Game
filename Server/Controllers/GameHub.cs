using Microsoft.AspNetCore.SignalR;
using Models.Game;
using Server.Services;

namespace Server.Controllers;

public class GameHub : Hub
{
    private const string RECIEVE_MSG = "RecieveMsg";

    private readonly DatabaseContext _dbContext;
    private readonly RoomService _roomService;

    public GameHub (DatabaseContext dbContext, RoomService roomService)
    {
        _dbContext = dbContext;
        _roomService = roomService;
    }

    public async void EnterRoom(Room currentRoom, Character character, int x, int y)
    {
        _roomService.RemoveCharacterFromRoom(character, currentRoom);

        await Clients.OthersInGroup(currentRoom.ToString()).SendAsync(RECIEVE_MSG, $"{character.Name} вышел в одну из дверей, и был таков.");

        var newRoom = await _roomService.GetRoom(x, y);

        _roomService.AddCharacterToRoom(character, newRoom);

        var roomDesc = _roomService.ConstructRoomDescription(newRoom, character.Name!);

        await Clients.Caller.SendAsync(RECIEVE_MSG, roomDesc);

        await Clients.OthersInGroup(newRoom.ToString()).SendAsync(RECIEVE_MSG, $"Вы видите как в комнату входит {character.Name}. " +
                                                                               $"На нём надет {character.Armor!.Title}, а в руках он держит {character.Weapon!.Title}");
    }

    public async void Spawn (Character character)
    {
        var room = await _roomService.GetRoom(character.CurrentX, character.CurrentY);

        AddPlayerToRoom(character, room);

        var roomDesc = _roomService.ConstructRoomDescription(room, character.Name!);

        await Clients.Caller.SendAsync(RECIEVE_MSG, roomDesc);

        await Clients.OthersInGroup(room.ToString()).SendAsync(RECIEVE_MSG, $"Неожиданно, прямо посреди комнаты появилась какая-то непонятная фигура, " +
                                                                            $"на которой надет {character.Armor!.Title}, а рядом лежит {character.Weapon!.Title}. " +
                                                                            $"Что-то подсказывает вам что эту фигуру зовут {character.Name}");
    }

    public async void PickUpLoot (Character character, Room room)
    {
        var moneyFound = room.MoneyIn;

        if (room.MoneyIn > 3)
            moneyFound = (int)(room.MoneyIn * Random.Shared.NextSingle());

        try {
            room.MoneyIn -= moneyFound;

            _dbContext.Rooms.Update(room);
            await _dbContext.SaveChangesAsync();
        }
        catch {
            throw;
        }

        await Clients.Caller.SendAsync("FoundMoney", moneyFound);

        await Clients.OthersInGroup(room.ToString()).SendAsync(RECIEVE_MSG, $"{character.Name} только что подобрал с пола {moneyFound} монет");
    }

    private async void RemovePlayerFromRoom (Character character, Room room)
    {
        _roomService.RemoveCharacterFromRoom(character, room);

        await Groups.RemoveFromGroupAsync(Context.ConnectionId, room.ToString());
    }

    private async void AddPlayerToRoom (Character character, Room room)
    {
        _roomService.AddCharacterToRoom(character, room);

        await Groups.AddToGroupAsync(Context.ConnectionId, room.ToString());
    }
}