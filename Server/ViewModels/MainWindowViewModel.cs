using Server.Commands;
using Server.Models;
using Server.Services;
using System.Collections.ObjectModel;

namespace Server.ViewModels
{
    internal class MainWindowViewModel : ViewModel
    {
        public ConnectionService ConnectionService { get; private set; }

        public StartServerCommand StartServerCommand { get; private set; }

        #region LoggerMessages
        private ObservableCollection<string>? _loggerMessages;

        public ObservableCollection<string>? LoggerMessages
        {
            get { return _loggerMessages; }
            private set { Set(ref _loggerMessages, value, nameof(LoggerMessages)); }
        }
        #endregion

        #region Clients
        private ObservableCollection<Client> _clients;

        public ObservableCollection<Client> Clients
        {
            get { return _clients; }
            private set { Set(ref _clients, value, nameof(Clients)); }
        } 
        #endregion

        public MainWindowViewModel()
        {
            ConnectionService = new ConnectionService("127.0.0.1", 8080);

            StartServerCommand = new StartServerCommand(this);
            LoggingService.OnMessageAdded += LoggingService_OnMessageAdded;
            ConnectionService.Clients.CollectionChanged += Clients_CollectionChanged;
        }

        private void Clients_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            Clients = new ObservableCollection<Client>(ConnectionService.Clients);
        }

        private void LoggingService_OnMessageAdded()
        {
            LoggerMessages = new ObservableCollection<string>(LoggingService.Messages);
        }
    }
}
