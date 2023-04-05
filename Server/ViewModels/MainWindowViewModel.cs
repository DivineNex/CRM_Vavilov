using Server.Commands;
using Server.Services;

namespace Server.ViewModels
{
    internal class MainWindowViewModel : ViewModel
    {
        public ConnectionService ConnectionService { get; private set; } 

        public StartServerCommand StartServerCommand { get; private set; }

        public MainWindowViewModel()
        {
            ConnectionService = new ConnectionService("127.0.0.1", 8080);

            StartServerCommand = new StartServerCommand(this);
        }
    }
}
