using UniRx;
using UnityEngine;

public class EventManager
{
    public Subject<ICommand> OnCommandExecuted { get; } = new Subject<ICommand>();
    public Subject<string> OnSoundPlayed { get; } = new Subject<string>();

    public void ExecuteCommand(ICommand command)
    {
        if (command == null)
        {
            Debug.LogError("Attempted to execute a null command.");
            return;
        }

        command.Execute();
        OnCommandExecuted.OnNext(command);
        EmitSound(command);
    }

    private void EmitSound(ICommand command)
    {
        switch (command)
        {
            case PlayerShootCommand:
                OnSoundPlayed.OnNext("shoot");
                break;
            case EnemyDamageCommand:
                OnSoundPlayed.OnNext("enemy_damage");
                break;
            case EnemyKilledCommand:
                OnSoundPlayed.OnNext("enemy_killed");
                break;
            default:
                Debug.Log("No sound for this command.");
                break;
        }
    }
}