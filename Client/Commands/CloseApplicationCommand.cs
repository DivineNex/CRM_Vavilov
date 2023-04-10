namespace Client.Commands
{
    internal class CloseApplicationCommand : Command
    {
        public override bool CanExecute(object? parameter)
        {
            return true;
        }

        public override void Execute(object? parameter)
        {
            App.Current.Shutdown();
        }
    }
}
