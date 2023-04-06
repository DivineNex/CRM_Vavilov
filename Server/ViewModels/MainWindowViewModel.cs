using Server.Commands;
using Server.Services;
using System.Collections.Generic;

namespace Server.ViewModels
{
    internal class MainWindowViewModel : ViewModel
    {
        public ConnectionService ConnectionService { get; private set; } 

        public StartServerCommand StartServerCommand { get; private set; }

        private List<string>? _loggerMessages;

        public List<string>? LoggerMessages
        {
            get { return _loggerMessages; }
            set { Set(ref _loggerMessages, value, nameof(LoggerMessages)); }
        }

        public MainWindowViewModel()
        {
            ConnectionService = new ConnectionService("127.0.0.1", 8080);

            StartServerCommand = new StartServerCommand(this);
            LoggingService.OnMessageAdded += LoggingService_OnMessageAdded;
        }

        private void LoggingService_OnMessageAdded()
        {
            LoggerMessages = LoggingService.Messages;
        }
    }
}
