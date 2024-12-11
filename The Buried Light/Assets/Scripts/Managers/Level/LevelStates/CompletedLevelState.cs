using UnityEngine;
using Cysharp.Threading.Tasks;

public class CompletedLevelState : LevelStateBase
{
    public override async UniTask OnStateEnterAsync(LevelManager levelManager)
    {
        await base.OnStateEnterAsync(levelManager);
        Debug.Log("Level completed. Transitioning to next phase or main menu.");

        // Handle completion logic
        if (LevelManager.HasMorePhases())
        {
            await LevelManager.SetStateAsync<PreparingLevelState>();
        }
        else
        {
            // If all phases are complete, transition to the main menu or idle state
            await LevelManager.SetStateAsync<IdleLevelState>();
        }
    }
}
