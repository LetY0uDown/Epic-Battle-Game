using Models;
using WPF_Client.Core.Tools;
using WPF_Client.Views;
using WPF_Client_Library;

namespace WPF_Client.Core.ViewModels;

internal sealed class LoginViewModel : ViewModel
{
    public LoginViewModel()
    {
        LoginCommand = new Command(async o =>
        {
            var requestString = HasAccount ? "Users/Login" : "Users/Registration";

            var user = new User(Login!, Password!);

            var returnedUser = await APIClient.PostAsync(requestString, user);

            if (returnedUser is not null)
            {
                App.CurrentUser = returnedUser;
                App.SwitchMainWindow<CharacterSelectionWindow>();
            }

        }, b => !string.IsNullOrWhiteSpace(Login)
                && !string.IsNullOrWhiteSpace(Password));
    }

    public string? Login { get; set; }

    public string? Password { get; set; }

    public bool HasAccount { get; set; }

    public Command LoginCommand { get; }
}