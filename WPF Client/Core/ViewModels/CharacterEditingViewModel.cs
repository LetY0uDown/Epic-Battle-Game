using Models.Game;
using WPF_Client_Library;

namespace WPF_Client.Core.ViewModels;

internal class CharacterEditingViewModel : ViewModel
{
    public CharacterEditingViewModel()
    {
        Character = App.CurrentUser!.CurrentCharacter!;

        SaveCommand = new(async o => {

        }, 
        b => {
            return !string.IsNullOrWhiteSpace(Character.Name) && !string.IsNullOrWhiteSpace(Character.Description);
        });
    }

    public Character Character { get; set; }

    public Command SaveCommand { get; } 
}