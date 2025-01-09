using UnityEngine;
using Zenject;
using UniRx;
using System;

public class PlayerEventsHandler : IInitializable, IDisposable
{
    private readonly PlayerEvents _playerEvents;
    private readonly LevelManager _levelManager;
    private readonly CompositeDisposable _disposables = new CompositeDisposable();

    [Inject]
    public PlayerEventsHandler(PlayerEvents playerEvents, LevelManager levelManager)
    {
        _playerEvents = playerEvents;
        _levelManager = levelManager;
    }

    public void Initialize()
    {
        _playerEvents.OnPlayerShot
            .Subscribe(_ => HandlePlayerShot())
            .AddTo(_disposables);

        _playerEvents.OnPlayerDeath
            .Subscribe(_ => HandlePlayerDeath())
            .AddTo(_disposables);
    }

    private void HandlePlayerShot()
    {

    }

    private void HandlePlayerDeath()
    {
        _levelManager.SetState(_levelManager.FailedLevelState);
    }


    public void Dispose()
    {
        _disposables.Dispose();
    }
}

