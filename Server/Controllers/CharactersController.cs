using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models.Game;
using Server.Services;

namespace Server.Controllers;

[ApiController, Route("[controller]")]
public sealed class CharactersController : ControllerBase
{
    private readonly DatabaseContext _dbContext;

    public CharactersController (DatabaseContext dbContext,)
    {
        _dbContext = dbContext;
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Character>> GetCharacter (int id)
    {
        var character = await _dbContext.Characters.Include("Armor").Include("Weapon")
            .FirstOrDefaultAsync(c => c.ID == id);

        return character switch {
            null => NotFound("Ошибка. Персонажа с таким ID не существует"),
            _ => character
        };       
    }

    [HttpPost]
    public async Task<ActionResult> CreateCharacter ([FromBody] Character character)
    {
        if (!ValidateName(character.Name!)) {
            return BadRequest("Имя персонажа уже занято. Попробуйте другое");
        }

        try {
        character.Weapon = await _dbContext.Weapons.FindAsync(1);
        character.Armor = await _dbContext.Armor.FindAsync(1);

        await _dbContext.Characters.AddAsync(character);
        await _dbContext.SaveChangesAsync();
        }
        catch {
            throw;
        }

        return Ok();
    }

    [HttpPut]
    public async Task<ActionResult> UpdateCharacter ([FromBody] Character character)
    {
        if (!ValidateName(character.Name!)) {
            return BadRequest("Имя персонажа уже занято. Попробуйте другое");
        }

        try {
        _dbContext.Characters.Update(character);
        await _dbContext.SaveChangesAsync();
        }
        catch {
            throw;
        }

        return Ok();
    }

    private bool ValidateName(string name)
    {
        return !_dbContext.Characters.Any(c => c.Name == name);
    }
}