using Models.Game;
using System.Collections.Generic;
using System.Linq;
using System;
using WPF_Client.Core.Tools;
using WPF_Client.Views;
using WPF_Client_Library;

namespace WPF_Client.Core.ViewModels;

internal sealed class CharacterSelectorViewModel : ViewModel
{
    public CharacterSelectorViewModel()
    {
        SetCharacters();

        CreateCharacterCommand = new(async o =>
        {
            var newChar = new Character(App.CurrentUser!.ID);

            newChar.CurrentX = Random.Shared.Next(-1, 2);
            newChar.CurrentY = Random.Shared.Next(-1, 2);

            App.CurrentUser!.CurrentCharacter = await APIClient.PostAsync("Characters", newChar);

            App.SwitchMainWindow<CharacterEditingWindow>();
        });

        EditCharacterCommand = new(o =>
        {
            App.CurrentUser!.CurrentCharacter = SelectedCharacter;

            App.SwitchMainWindow<CharacterEditingWindow>();

        }, b => SelectedCharacter is not null);

        PlayCommand = new(async o =>
        {
            App.CurrentUser!.CurrentCharacter = SelectedCharacter;

            App.CurrentUser = await APIClient.PutAsync("Users", App.CurrentUser);

            App.SwitchMainWindow<GameClientWindow>();

        }, b => SelectedCharacter is not null);
    }

    public Character? SelectedCharacter { get; set; }

    public List<Character>? Characters { get; set; }

    public Command PlayCommand { get; }
    public Command EditCharacterCommand { get; }
    public Command CreateCharacterCommand { get; }

    private async void SetCharacters()
    {
        var collection = await APIClient.GetCollectionAsync<Character>($"Characters/User/{App.CurrentUser!.ID}");

        Characters = collection.ToList();
    }
}