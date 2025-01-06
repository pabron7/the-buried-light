using UnityEngine;

public class InProgressLevelState : LevelStateBase
{
    public override void OnStateEnter(LevelManager levelManager)
    {
        levelManager.StartNextPhase();
    }
}

