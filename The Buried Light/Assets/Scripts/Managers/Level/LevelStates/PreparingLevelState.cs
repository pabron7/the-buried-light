using UnityEngine;

public class PreparingLevelState : LevelStateBase
{
    public override void OnStateEnter(LevelManager levelManager)
    {
        base.OnStateEnter(levelManager);
        Debug.Log("Preparing level...");
        // Simulate preparation (e.g., async loading logic can go here)
        levelManager.SetState(new InProgressLevelState());
    }
}
