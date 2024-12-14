using UnityEngine;

public class IdleLevelState : LevelStateBase
{
    public override void OnStateEnter(LevelManager levelManager)
    {
        base.OnStateEnter(levelManager);
        Debug.Log("Idle state entered. Resetting wave pool.");
        levelManager.ResetWavePool();
    }
}
