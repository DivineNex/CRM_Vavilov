using Server.ViewModels;

namespace Server.Commands
{
    internal class StartServerCommand : Command
    {
        private readonly MainWindowViewModel _mainWindowViewModel;

        public StartServerCommand(MainWindowViewModel mainWindowViewModel)
        {
            _mainWindowViewModel = mainWindowViewModel;
        }

        public override bool CanExecute(object? parameter)
        {
            return !_mainWindowViewModel.ConnectionService.Active;
        }

        public override void Execute(object? parameter)
        {
            _mainWindowViewModel.ConnectionService.StartServer();
        }
    }
}
