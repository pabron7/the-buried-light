using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

public class CompletedLevelState : LevelStateBase
{
    [Inject] GameProgressStore _gameProgressStore;
    [Inject] SaveManager _saveManager;
    public override async void OnStateEnter(LevelManager levelManager)
    {
        base.OnStateEnter(levelManager);
        Debug.Log("CompletedLevelState: Level completed. Waiting for 2 seconds before transitioning to GameOverState.");

        // Global Level Value is Increased
        _gameProgressStore.CurrentLevel.Value++;

        //help me call save manager's SaveChunkAsync function here to save the current game progress

        await UniTask.Delay(2000);       // Wait for 2 seconds

        // Transition to GameOverState
        levelManager.UpdateGameState<GameOverState>();
        levelManager.SetState(new IdleLevelState());

        Debug.Log("CompletedLevelState: Transitioned to GameOverState.");
    }
}