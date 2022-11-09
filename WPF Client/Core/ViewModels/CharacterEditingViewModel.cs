using Models.Game;
using WPF_Client.Core.Tools;
using WPF_Client.Views;
using WPF_Client_Library;

namespace WPF_Client.Core.ViewModels;

internal class CharacterEditingViewModel : ViewModel
{
    public CharacterEditingViewModel()
    {
        Character = App.CurrentUser!.CurrentCharacter!;

        SaveCommand = new(async o => {
            App.CurrentUser!.CurrentCharacter = await APIClient.PutAsync("Characters", Character);

            App.SwitchMainWindow<CharacterSelectionWindow>();
        }, 
        b => {
            return !string.IsNullOrWhiteSpace(Character.Name) && !string.IsNullOrWhiteSpace(Character.Description);
        });
    }

    public Character Character { get; set; }

    public Command SaveCommand { get; } 
}