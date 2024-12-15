using UnityEngine;

public class PreparingLevelState : LevelStateBase
{
    public override void OnStateEnter(LevelManager levelManager)
    {
        base.OnStateEnter(levelManager);
        Debug.Log("Preparing level...");

        levelManager.SetState(new InProgressLevelState());
    }
}
