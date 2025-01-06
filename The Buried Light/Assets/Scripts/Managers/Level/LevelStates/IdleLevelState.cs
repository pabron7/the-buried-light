using UnityEngine;

public class IdleLevelState : LevelStateBase
{
    public override void OnStateEnter(LevelManager levelManager)
    {
        // Reset all waves using PhaseManager
        levelManager.PhaseManager.ResetWaves();
    }
}
