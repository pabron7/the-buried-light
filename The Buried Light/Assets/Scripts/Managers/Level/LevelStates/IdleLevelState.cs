using UnityEngine;

public class IdleLevelState : LevelStateBase
{
    public override void OnStateEnter(LevelManager levelManager)
    {
        Debug.Log("IdleLevelState: Entered Idle State.");

        // Reset all waves using PhaseManager
        levelManager.PhaseManager.ResetWaves();
    }

    public override void OnUpdate()
    {
        // Logic for Idle State updates, if any
    }

    public override void OnStateExit()
    {
        Debug.Log("IdleLevelState: Exiting Idle State.");
    }
}
