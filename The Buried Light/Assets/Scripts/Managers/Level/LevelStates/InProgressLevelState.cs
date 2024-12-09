using UnityEngine;

public class InProgressLevelState : LevelStateBase
{
    public override void OnStateEnter(LevelManager levelManager)
    {
        base.OnStateEnter(levelManager);
        Debug.Log("LevelManager: InProgress state.");
        LevelManager.InitializeWaves();

    
    }
}
