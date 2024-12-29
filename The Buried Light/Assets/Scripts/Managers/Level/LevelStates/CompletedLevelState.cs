using Cysharp.Threading.Tasks;
using UnityEngine;

public class CompletedLevelState : LevelStateBase
{
    public override async void OnStateEnter(LevelManager levelManager)
    {
        base.OnStateEnter(levelManager);
        Debug.Log("CompletedLevelState: Level completed. Waiting for 2 seconds before transitioning to GameOverState.");

        await UniTask.Delay(2000);       // Wait for 2 seconds

        // Transition to GameOverState
        levelManager.UpdateGameState<GameOverState>();
        levelManager.SetState(new IdleLevelState());

        Debug.Log("CompletedLevelState: Transitioned to GameOverState.");
    }
}