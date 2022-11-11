using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models;
using Server.Services;

namespace Server.Controllers;

[ApiController, Route("[controller]")]
public class UsersController : ControllerBase
{
    private readonly DatabaseContext _dbContext;

    public UsersController (DatabaseContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpPost("Registration")]
    public async Task<ActionResult<User>> Register ([FromBody] User user)
    {
        if (UsernameExist(user.Username)) {
            return BadRequest("Пользователь с таким ником уже существует. Попробуй другой ник");
        }

        try {
        await _dbContext.Users.AddAsync(user);
        await _dbContext.SaveChangesAsync();
        }
        catch {
            throw;
        }

        return user;
    }

    [HttpPost("Login")]
    public async Task<ActionResult<User>> Login ([FromBody] User user)
    {
        var findedUser = await _dbContext.Users.Include("CurrentCharacter").FirstOrDefaultAsync(u => u.Username == user.Username);

        if (findedUser is null) {
            return BadRequest("Пользователя с таким именем не существует");
        }

        if (findedUser.Password != user.Password) {
            return BadRequest("Не верный пароль. Попробуйте ещё раз");
        }

        return findedUser;
    }

    [HttpPut]
    public async Task<ActionResult> Put (User user)
    {
        try {
            _dbContext.Users.Update(user);
            await _dbContext.SaveChangesAsync();
        }
        catch {
            throw;
        }

        return Ok();
    }

    [HttpGet]
    public async Task<IEnumerable<User>> Users()
    {
        return _dbContext.Users.Include(u => u.CurrentCharacter)
                               .Include(u => u.CurrentCharacter!.Weapon)
                               .Include(u => u.CurrentCharacter!.Armor);
    }

    private bool UsernameExist(string username)
    {
        return _dbContext.Users.Any(u => u.Username == username);
    }
}