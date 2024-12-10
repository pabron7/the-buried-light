using System.Collections;
using UnityEngine;

public class PreparingLevelState : LevelStateBase
{
    public override void OnStateEnter(LevelManager levelManager)
    {
        base.OnStateEnter(levelManager);
        LevelManager.StartCoroutine(PrepareLevel());
    }

    private IEnumerator PrepareLevel()
    {
        Debug.Log("Preparing level...");

        // Simulate preparation work
        yield return new WaitForSeconds(2f);

        // Transition to the in-progress state to start waves
        LevelManager.SetState<InProgressLevelState>();
    }
}
