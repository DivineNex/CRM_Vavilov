using Client.Views;
using System.Windows;

namespace Client
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            MainWindowView mainWindowView = new MainWindowView();
            mainWindowView.Show();

            LoginWindowView loginWindowView = new LoginWindowView();
            loginWindowView.Owner = mainWindowView;
            loginWindowView.UseNoneWindowStyle = true;
            loginWindowView.ShowDialog();

            base.OnStartup(e);
        }
    }
}
