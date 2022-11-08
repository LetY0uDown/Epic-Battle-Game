using Models.Abstract;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models.Game;

[Table(nameof(Item))]
public class Item : Model
{
    public string? Title { get; set; }

    public string? Description { get; set; }
}