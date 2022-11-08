using Models.Abstract;

namespace Models.Game;

public class Room : Model
{
    public Room (int x, int y)
    {
        X = x;
        Y = y;

        CurrentStatus = Status.Peace;
    }

    public int X { get; init; }

    public int Y { get; init; }

    public int MoneyIn { get; set; }

    public string? Description { get; set; }

    public Status CurrentStatus { get; set; }

    public override string ToString ()
    {
        return $"Room[{X}-{Y}]";
    }

    public enum Status
    {
        Peace,
        Battle
    }
}