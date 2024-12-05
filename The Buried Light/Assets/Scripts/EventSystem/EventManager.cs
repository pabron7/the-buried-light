using Zenject;
using UniRx;
using UnityEngine;

public class EventManager
{
    public Subject<ICommand> OnCommandExecuted { get; } = new Subject<ICommand>();

    private readonly SoundManager _soundManager;
    private readonly CompositeDisposable _disposables = new CompositeDisposable();

    [Inject]
    public EventManager(SoundManager soundManager)
    {
        _soundManager = soundManager;

        // Subscribe to command execution to trigger sound effects
        OnCommandExecuted
            .Subscribe(HandleCommand)
            .AddTo(_disposables); // Use CompositeDisposable to manage subscription lifecycle
    }

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

    private void HandleCommand(ICommand command)
    {
        switch (command)
        {
            case PlayerShootCommand:
                _soundManager.PlayShootSound();
                break;
            case EnemyDamageCommand:
                _soundManager.PlayEnemyDamageSound();
                break;
            case EnemyKilledCommand:
                _soundManager.PlayEnemyKilledSound();
                break;
        }
    }

    ~EventManager()
    {
        _disposables.Dispose(); // Clean up subscriptions when EventManager is destroyed
        Debug.Log("EventManager disposed");
    }
}
