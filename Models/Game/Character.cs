using Models.Abstract;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models.Game;

[Table(nameof(Character))]
public class Character : Model
{
    public Character(int creatorID)
    {
        CreatorID = creatorID;
    }

    private const int DEFAULT_MAX_HP = 10;

    public int CreatorID { get; set; }

    public int CurrentX { get; set; }

    public int CurrentY { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }

    public int Money { get; set; }

    public int MaxHP { get; set; } = DEFAULT_MAX_HP;

    public int CurrentHP { get; set; } = DEFAULT_MAX_HP;

    public Weapon? Weapon { get; set; }

    public Armor? Armor { get; set; }
}