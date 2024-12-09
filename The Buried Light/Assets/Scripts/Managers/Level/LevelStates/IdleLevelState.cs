using UnityEngine;

public class IdleLevelState : LevelStateBase
{
    public override void OnStateEnter(LevelManager levelManager)
    {
        base.OnStateEnter(levelManager);
        Debug.Log("Level is idle. Ready to initialize or start.");
    }
}
