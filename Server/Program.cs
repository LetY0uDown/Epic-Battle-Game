using Server.Controllers;
using Server.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<DatabaseContext>();
builder.Services.AddScoped<RoomService>();
builder.Services.AddScoped<BattleService>();

builder.Services.AddSignalR();

var app = builder.Build();

if (app.Environment.IsDevelopment()) {
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.MapHub<GameHub>("Game");

app.Run();