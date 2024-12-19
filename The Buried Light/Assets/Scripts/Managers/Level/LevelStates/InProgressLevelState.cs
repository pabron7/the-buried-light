using UnityEngine;

public class InProgressLevelState : LevelStateBase
{
    public override void OnStateEnter(LevelManager levelManager)
    {
        Debug.Log("InProgress state entered.");
        levelManager.StartNextPhase();
    }
}

