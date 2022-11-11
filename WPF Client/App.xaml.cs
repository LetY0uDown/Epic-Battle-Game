using Models;
using System.Windows;
using WPF_Client.Views;

namespace WPF_Client;

public partial class App : Application
{
    internal static User? CurrentUser { get; set; }

    internal static void SwitchMainWindow(Window window)
    {
        Current.MainWindow?.Close();

        Current.MainWindow = window;

        Current.MainWindow.Show();
    }

    private void Application_Startup(object sender, StartupEventArgs e)
    {
        Current.MainWindow = new LoginWindow();
        Current.MainWindow.Show();
    }
}