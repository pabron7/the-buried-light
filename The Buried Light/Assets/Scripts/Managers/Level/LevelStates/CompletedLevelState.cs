using UnityEngine;

public class CompletedLevelState : LevelStateBase
{
    public override void OnStateEnter(LevelManager levelManager)
    {
        base.OnStateEnter(levelManager);
        Debug.Log("Level completed. Transitioning to next phase or main menu.");
    }
}