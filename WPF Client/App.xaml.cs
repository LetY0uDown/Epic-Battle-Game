using Models;
using System;
using System.Windows;
using WPF_Client.Views;

namespace WPF_Client;

public partial class App : Application
{
    internal static User? CurrentUser { get; set; }

    internal static void SwitchMainWindow<T>() where T : Window
    {
        Current.MainWindow?.Close();

        Current.MainWindow = Activator.CreateInstance<T>();

        Current.MainWindow.Show();
    }

    private void Application_Startup(object sender, StartupEventArgs e)
    {
        SwitchMainWindow<LoginWindow>();
    }
}