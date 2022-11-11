using Microsoft.AspNetCore.Mvc;
using Models.Game;
using Server.Services;

namespace Server.Controllers;

[ApiController, Route("[controller]")]
public class ItemsController : ControllerBase
{
    private readonly DatabaseContext _dbContext;

    public ItemsController(DatabaseContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpGet("Weapons/{maxPrice:int}")]
    public Task<List<Weapon>> Weapons(int maxPrice)
    {
        return Task.FromResult(_dbContext.Weapons.Where(w => w.Price <= maxPrice).ToList());
    }
    
    [HttpGet("Armor/{maxPrice:int}")]
    public Task<List<Armor>> Armor(int maxPrice)
    {
        return Task.FromResult(_dbContext.Armor.Where(a => a.Price <= maxPrice).ToList());
    }
}