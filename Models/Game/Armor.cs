using System.ComponentModel.DataAnnotations.Schema;

namespace Models.Game;

[Table(nameof(Armor))]
public class Armor : Item
{
    public int Resistance { get; set; }
}