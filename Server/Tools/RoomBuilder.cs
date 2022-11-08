using Models.Game;
using System.Text;

namespace Server.Tools;

internal class RoomBuilder
{
    private readonly string[] _randomDecorations = new string[] {
        "потрёпаный синий ковёр", "зелёный ковёр", "очень старый на вид красный ковёр",
        "на стене висит потухший факел", "много паутины по углам",
        "потрескавшиеся от времени стены"
    };

    private readonly string[] _randomFurniture = new string[] {
        "старое, ободраное кресло посреди комнаты",
        "дряхлый книжный шкаф в углу",
        "полуразвалившийся стол где-то возле стены",
        "маленькая табуретка с висящей на ней тряпкой",
        "пыльный диванчик с большой дырой посередине",
        "небольших размеров столик, с каким-то бумагами на нём",
        "довольно большой деревянный стол, стоящий вдоль одной из стен"
    };

    private readonly Room _room;
    private readonly StringBuilder _descriptionBuilder;

    internal RoomBuilder (int x, int y)
    {
        _room = new Room(x, y) {
            Description = string.Empty
        };

        _descriptionBuilder = new StringBuilder();
    }

    internal RoomBuilder WithMoney(int minMoney, int maxMoney)
    {
        var moneyCount = Random.Shared.Next(minMoney, maxMoney + 1);

        _room.MoneyIn = moneyCount;

        return this;
    }

    internal RoomBuilder WithDecorations ()
    {
        _descriptionBuilder.Append("В этой комнате ");
        _descriptionBuilder.Append(_randomDecorations.RandomElement());

        _descriptionBuilder.Append(". ");

        return this;
    }

    internal RoomBuilder WithFurniture ()
    {
        _descriptionBuilder.Append("Из мебели здесь есть ");
        _descriptionBuilder.Append(_randomFurniture.RandomElement());

        if (Random.Shared.Next(0, 2) == 0) {
            _descriptionBuilder.Append(", а также ");
            _descriptionBuilder.Append(_randomFurniture.RandomElement());
        }

        _descriptionBuilder.Append(". ");

        return this;
    }

    internal Room Build ()
    {
        _room.Description = !_room.Description!.Equals(string.Empty) ? "Пустая комната. Здесь темно и холодно. И хочется кушать"
                                                                     : _descriptionBuilder.ToString();

        return _room;
    }
}