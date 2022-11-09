using Microsoft.AspNetCore.SignalR.Client;
using Models.Game;
using System.Collections.Generic;
using WPF_Client.Core.Tools;
using WPF_Client_Library;

namespace WPF_Client.Core.ViewModels;

internal sealed class GameViewModel : ViewModel
{
    private HubConnection? _hub;

    public GameViewModel()
    {
        // CurrentCharacter = App.CurrentUser!.CurrentCharacter!;

        // SetupHub();

        CurrentCharacter = new Character(0)
        {
            Name = "Oleg",
            CurrentHP = 8,
            Weapon = new Weapon()
            {
                Title = "Палка",
                MinDamage = 0,
                MaxDamage = 7
            },
            Armor = new Armor()
            {
                Title = "Кожа",
                Resistance = 2
            }
        };
    }

    public List<string> Actions { get; } = new();

    public Command PickLootCommand { get; }
    public Command MoveOnCommand { get; }

    public Character CurrentCharacter { get; }

    private async void SetupHub()
    {
        _hub = new HubConnectionBuilder().WithUrl(Config.GetValue("host") + "Game")
                                         .WithAutomaticReconnect()
                                         .Build();

        await _hub.StartAsync();

        await _hub.SendAsync("Spawn", CurrentCharacter);

        _hub.On<string>("RecieveMsg", msg => {
            Actions.Add(msg);
        });

        _hub.On<Character>("UpdateCharacter", character => {
            App.CurrentUser!.CurrentCharacter = character;
        });

        _hub.On<int>("FoundMoney", money => {
            CurrentCharacter.Money += money;
        });
    }
}