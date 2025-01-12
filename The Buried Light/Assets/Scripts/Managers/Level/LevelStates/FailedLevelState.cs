using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

public class FailedLevelState : LevelStateBase
{
    [Inject] private GameProgressStore _gameProgressStore;
    [Inject] private SaveManager _saveManager;
    [Inject] private LevelResult _levelResult;

    public override async void OnStateEnter(LevelManager levelManager)
    {
        base.OnStateEnter(levelManager);

        // Mark the level as failed
        _levelResult.SetFailed();

        // Save game progress
        await _saveManager.SaveChunkAsync("GameProgress", _gameProgressStore.ToGameProgress());

        await UniTask.Delay(2000);

        // Transition to GameOverState
        levelManager.UpdateGameState<GameOverState>();
    }
}
