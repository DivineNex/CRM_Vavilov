using Server.Commands;
using Server.Services;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Server.ViewModels
{
    internal class MainWindowViewModel : ViewModel
    {
        public ConnectionService ConnectionService { get; private set; } 

        public StartServerCommand StartServerCommand { get; private set; }

        public List<string> LoggerMessages { get; private set; }

        public MainWindowViewModel()
        {
            ConnectionService = new ConnectionService("127.0.0.1", 8080);

            StartServerCommand = new StartServerCommand(this);
            LoggingService.OnMessageAdded += LoggingService_OnMessageAdded;
        }

        private void LoggingService_OnMessageAdded()
        {
            LoggerMessages = LoggingService.Messages;
            OnPropertyChanged(nameof(LoggerMessages));
        }
    }
}
