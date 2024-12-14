using UnityEngine;

public class CompletedLevelState : LevelStateBase
{
    public override void OnStateEnter(LevelManager levelManager)
    {
        base.OnStateEnter(levelManager);
        Debug.Log("Level completed. Transitioning to next phase or main menu.");

        // Handle completion logic
        if (LevelManager.HasMorePhases())
        {
            LevelManager.SetState<PreparingLevelState>();
        }
        else
        {
            // If all phases are complete, transition to the main menu or game over state
            LevelManager.SetState<IdleLevelState>();
        }
    }
}
