using Microsoft.AspNetCore.SignalR.Client;
using Models.Game;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using WPF_Client.Core.Tools;
using WPF_Client_Library;

namespace WPF_Client.Core.ViewModels;

internal sealed class GameViewModel : ViewModel
{
    private HubConnection? _hub;

    private bool _isAbleToTurn;

    public GameViewModel ()
    {
        // CurrentCharacter = App.CurrentUser!.CurrentCharacter!;

        // SetupHub();

        CurrentCharacter = new Character(0) {
            Name = "Oleg",
            CurrentHP = 8,
            Weapon = new Weapon() {
                Title = "Палка",
                MinDamage = 0,
                MaxDamage = 7
            },
            Armor = new Armor() {
                Title = "Кожа",
                Resistance = 2
            }
        };
    }

    public Visibility BattleControlsVisibility { get; private set; } = Visibility.Collapsed;
    public Visibility NonBattleControlsVisibility { get; private set; }

    public ObservableCollection<string> Actions { get; } = new();

    #region Lots of commands
    public Command MakeAction { get; private set; }

    public Command PickLootCommand { get; private set; }
    public Command MoveOnCommand { get; private set; }
    #endregion
    
    public Character CurrentCharacter { get; }

    private void InitializeCommands ()
    {
        MakeAction = new(async o => {
            var action = (BattleAction.Type)int.Parse(o.ToString()!);

            // Add target selection
            await _hub!.SendAsync("MakeTurn", CurrentCharacter, action);

        }, b => _isAbleToTurn);
    }

    private async void SetupHub ()
    {
        _hub = new HubConnectionBuilder().WithUrl(Config.GetValue("host") + "Game")
                                         .WithAutomaticReconnect()
                                         .Build();

        await _hub.StartAsync();

        await _hub.SendAsync("Spawn", CurrentCharacter);

        _hub.On<string, Room>("EnterRoom", (msg, room) => {
            Actions.Add(msg);

            SwitchBattleStatus(room.CurrentStatus);
        });

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

    private void SwitchBattleStatus (Room.Status status)
    {
        if (status == Room.Status.Battle) {
            BattleControlsVisibility = Visibility.Visible;
            NonBattleControlsVisibility = Visibility.Collapsed;
        }
        else {
            BattleControlsVisibility = Visibility.Collapsed;
            NonBattleControlsVisibility = Visibility.Visible;
        }
    }
}