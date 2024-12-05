using UniRx;

public class EventManager
{
    // Reactive Subject for commands
    public Subject<ICommand> OnCommandExecuted { get; } = new Subject<ICommand>();

    /// <summary>
    /// Executes a command and notifies subscribers.
    /// </summary>
    public void ExecuteCommand(ICommand command)
    {
        if (command == null)
        {
            UnityEngine.Debug.LogError("Attempted to execute a null command.");
            return;
        }

        command.Execute();
        OnCommandExecuted.OnNext(command);
    }
}
