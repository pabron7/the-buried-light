using Zenject;
using UniRx;
using Cysharp.Threading.Tasks;
using UnityEngine;
using System;

public class SaveEventHandler : IInitializable, IDisposable
{
    private readonly SaveManager _saveManager;
    private readonly GameEvents _gameEvents;
    private readonly CompositeDisposable _disposables = new CompositeDisposable();

    [Inject]
    public SaveEventHandler(SaveManager saveManager, GameEvents gameEvents)
    {
        _saveManager = saveManager;
        _gameEvents = gameEvents;
    }

    public void Initialize()
    {
        Debug.Log("SaveEventHandler initialized.");

        _gameEvents.OnLevelEnd.Subscribe(_ => SavePlayerStatsAndProgressAsync().Forget()).AddTo(_disposables); 
    }

    /// <summary>
    /// Saves all data when the game is over.
    /// </summary>
    private async UniTask SaveAllDataAsync()
    {
        Debug.Log("SaveEventHandler: Saving all data...");
        await _saveManager.SaveAllAsync();
    }

    /// <summary>
    /// Saves player stats and game progress when a level ends.
    /// </summary>
    private async UniTask SavePlayerStatsAndProgressAsync()
    {
        Debug.Log("SaveEventHandler: Saving player stats and game progress...");
        await _saveManager.SaveChunkAsync("PlayerStats", _saveManager.PlayerStatsStore.ToPlayerStats());
        await _saveManager.SaveChunkAsync("GameProgress", _saveManager.GameProgressStore.ToGameProgress());
    }

    /// <summary>
    /// Saves only the game progress when a phase ends.
    /// </summary>
    private async UniTask SaveGameProgressAsync()
    {
        Debug.Log("SaveEventHandler: Saving game progress...");
        await _saveManager.SaveChunkAsync("GameProgress", _saveManager.GameProgressStore.ToGameProgress());
    }

    public void Dispose()
    {
        _disposables.Dispose();
        Debug.Log("SaveEventHandler disposed.");
    }
}
