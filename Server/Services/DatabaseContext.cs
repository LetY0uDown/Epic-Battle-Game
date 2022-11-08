using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Models;
using Models.Game;

namespace Server.Services;

public sealed class DatabaseContext : DbContext
{
    private readonly string _connectionString;

    public DatabaseContext (IConfiguration configuration)
    {
        _connectionString = configuration["ConnectionStrings:DefaultConnection"];

        Database.EnsureCreated();
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Character> Characters { get; set; }
    public DbSet<Item> Items { get; set; }
    public DbSet<Armor> Armor { get; set; }
    public DbSet<Weapon> Weapons { get; set; }
    public DbSet<Room> Rooms { get; set; }

    protected override void OnConfiguring (DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseMySql(_connectionString, ServerVersion.AutoDetect(_connectionString));
        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating (ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Armor>(ConfigureArmor);

        modelBuilder.Entity<Weapon>(ConfigureWeapon);
    }

    private void ConfigureWeapon(EntityTypeBuilder<Weapon> builder)
    {
        builder.HasData(
            new Weapon {
                Title = "Деревянная доска",
                Description = "Прогнившая деревянная доска. Несмотря на ужасный внешний вид, весьма страшное оружие",
                AttackBonus = 0,
                MinDamage = 0,
                MaxDamage = 4
            },
            new Weapon {
                Title = "Камень",
                Description = "Увесистый серо-красный булыжник, отколовшийся от какой-то скалы. Вполне может сломать противнику не одну кость",
                AttackBonus = 2,
                MinDamage = 2,
                MaxDamage = 6
            },
            new Weapon {
                Title = "Металлический кинжал",
                Description = "Короткий кинжал, сделаный из металла. Немного поржавел со временем, но всё ещё неплох",
                AttackBonus = 3,
                MinDamage = 2,
                MaxDamage = 8
            },
            new Weapon {
                Title = "Короткий меч",
                Description = "Небольшой стальной меч прямиком из рыцарских романов. Поможет вам сделать из противника колбасу. Если повезёт",
                AttackBonus = 5,
                MinDamage = 6,
                MaxDamage = 10
            });
    }

    private void ConfigureArmor(EntityTypeBuilder<Armor> builder)
    {
        builder.HasData(
           new Armor {
               Title = "Кожаная куртка",
               Description = "Лёгкая кожаная куртка коричневого цвета. Неплохо защищает разве что от слабого ветра",
               Resistance = 8
           },
           new Armor {
               Title = "Рубаха",
               Description = "Старая клетчатая рубаха. Похоже, что когда-то её носил какой-то дед, но теперь она ему не особо нужна",
               Resistance = 10
           },
           new Armor {
               Title = "Шерстяной свитер",
               Description = "Серый шерстяной свитер, который некогда связала чья-то добрая бабушка. Может здорово помочь в мороз",
               Resistance = 12
           },
           new Armor {
               Title = "Кожаный нагрудник",
               Description = "Клёпаный нагрудник, созданый из плотных кусков кожи. Хорошо защищает от тупых ударов тупыми предметами",
               Resistance = 14
           },
           new Armor {
               Title = "Железная броня",
               Description = "Добротный металлическйи доспех, который хорошо защитит вас от серъёзных ударов. Но удобным его не назвать..",
               Resistance = 16
           }
       );
    }
}