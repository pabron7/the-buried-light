using UnityEngine;

public class CompletedLevelState : LevelStateBase
{
    public override void OnStateEnter(LevelManager levelManager)
    {
        base.OnStateEnter(levelManager);
        Debug.Log("LevelManager: Level completed!");
    }
}
