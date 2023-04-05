using Client.Commands;
using Client.Services;

namespace Client.ViewModels
{
    internal class MainWindowViewModel : ViewModel
    {
        public ConnectionService ConnectionService { get; private set; }

        public ConnectToServerCommand ConnectToServerCommand { get; private set; }

        public MainWindowViewModel()
        {
            ConnectionService = new ConnectionService("127.0.0.1", 8080);

            ConnectToServerCommand = new ConnectToServerCommand(this);
        }
    }
}
