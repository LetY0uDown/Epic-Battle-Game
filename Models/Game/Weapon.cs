using System.ComponentModel.DataAnnotations.Schema;

namespace Models.Game;

[Table(nameof(Weapon))]
public class Weapon : Item
{
    public int AttackBonus { get; set; }

    public int MaxDamage { get; init; }

    public int MinDamage { get; init; }

    public int Damage => Random.Shared.Next(MinDamage, MaxDamage + 1);

    public override string ToString()
    {
        return $"{Title} - {(MinDamage + MaxDamage) / 2} ср. урон";
    }
}