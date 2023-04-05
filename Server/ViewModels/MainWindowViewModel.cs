using Server.Services;

namespace Server.ViewModels
{
    internal class MainWindowViewModel : ViewModel
    {
        private readonly ConnectionService _connectionService;

        public MainWindowViewModel()
        {
            _connectionService = new ConnectionService();
        }
    }
}
