using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

public class CompletedLevelState : LevelStateBase
{
    [Inject] GameProgressStore _gameProgressStore;
    [Inject] SaveManager _saveManager;
    [Inject] GameEvents _gameEvents;

    public override async void OnStateEnter(LevelManager levelManager)
    {
        base.OnStateEnter(levelManager);

        // Global Level Value is Increased
        _gameProgressStore.CurrentLevel.Value++;

        // Call Level Completed event
        _gameEvents.NotifyLevelEnd();

        await UniTask.Delay(2000);       // Wait for 2 seconds

        // Transition to GameOverState
        levelManager.UpdateGameState<GameOverState>();
        levelManager.SetState(new IdleLevelState());
    }
}