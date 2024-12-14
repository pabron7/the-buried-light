using UnityEngine;

public class CompletedLevelState : LevelStateBase
{
    public override void OnStateEnter(LevelManager levelManager)
    {
        base.OnStateEnter(levelManager);
        Debug.Log("Completed state entered. Transitioning to Idle or Preparing.");

        if (levelManager.CurrentState is InProgressLevelState)
        {
            levelManager.SetState(new IdleLevelState());
        }
    }
}
