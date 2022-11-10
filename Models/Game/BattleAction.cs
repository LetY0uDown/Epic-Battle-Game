using Models.Abstract;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models.Game;

[Table(nameof(BattleAction))]
public class BattleAction : Model
{
    public int RoomID { get; set; }

    public Character Sender { get; set; }

    public Character? Target { get; set; }

    public Type ActionType { get; set; }

    public enum Type
    {
        Attack,
        Defend,
        Heal,
        Swashbuckle,
        Nuke
    }
}