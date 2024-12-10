using UniRx;
using UnityEngine;

public class EventManager
{
    public Subject<ICommand> OnCommandExecuted { get; } = new Subject<ICommand>();

    public void ExecuteCommand(ICommand command)
    {
        if (command == null)
        {
            Debug.LogError("Attempted to execute a null command.");
            return;
        }

        command.Execute();
        OnCommandExecuted.OnNext(command);
    }
}
