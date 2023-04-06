using Client.ViewModels;

namespace Client.Commands
{
    internal class DisconnectFromServerCommand : Command
    {
        private readonly MainWindowViewModel _mainWindowViewModel;

        public DisconnectFromServerCommand(MainWindowViewModel mainWindowViewModel)
        {
            _mainWindowViewModel = mainWindowViewModel;
        }

        public override bool CanExecute(object? parameter)
        {
            return _mainWindowViewModel.ConnectionService.Connected;
        }

        public override void Execute(object? parameter)
        {
            _mainWindowViewModel.ConnectionService.DisconnectFromServer();
        }
    }
}
