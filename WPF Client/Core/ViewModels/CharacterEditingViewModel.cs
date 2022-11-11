using Models.Game;
using System.Collections.Generic;
using System.Linq;
using WPF_Client.Core.Tools;
using WPF_Client.Views;
using WPF_Client_Library;

namespace WPF_Client.Core.ViewModels;

internal class CharacterEditingViewModel : ViewModel
{
    public CharacterEditingViewModel()
    {
        SetData();

        SaveCommand = new(async o => {
            Character!.Weapon = SelectedWeapon;
            Character!.Armor = SelecetedArmor;

            App.CurrentUser!.CurrentCharacter = await APIClient.PutAsync("Characters", Character);

            App.SwitchMainWindow<CharacterSelectionWindow>();
        },
        b => CanSave());
    }

    public Weapon? SelectedWeapon { get; set; }

    public Armor? SelecetedArmor { get; set; }

    public List<Armor>? Armor { get; private set; }

    public List<Weapon>? Weapons { get; private set; }

    public Character Character { get; set; }

    public Command SaveCommand { get; }

    private async void SetData()
    {
        Character = App.CurrentUser!.CurrentCharacter!;

        var armor = await APIClient.GetCollectionAsync<Armor>("Items/Armor/", Character.Money);
        Armor = armor.ToList();

        var weapons = await APIClient.GetCollectionAsync<Weapon>("Items/Weapons/", Character.Money);
        Weapons = weapons.ToList();
    }

    private bool CanSave()
    {
        return !string.IsNullOrWhiteSpace(Character.Name)
               && !string.IsNullOrWhiteSpace(Character.Description)
               && SelecetedArmor is not null
               && SelectedWeapon is not null;
    }
}