using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

public class FailedLevelState : LevelStateBase
{
    [Inject] GameProgressStore _gameProgressStore;
    [Inject] SaveManager _saveManager;
    [Inject] GameEvents _gameEvents;

    public override async void OnStateEnter(LevelManager levelManager)
    {
        base.OnStateEnter(levelManager);
        levelManager.IsLevelFailed = true;

        // Notify that the level has ended due to failure
        _gameEvents.NotifyLevelEnd();

        await _saveManager.SaveChunkAsync("GameProgress", _gameProgressStore.ToGameProgress());

        await UniTask.Delay(2000);

        // Transition to GameOverState
        levelManager.UpdateGameState<GameOverState>();
    }
}
