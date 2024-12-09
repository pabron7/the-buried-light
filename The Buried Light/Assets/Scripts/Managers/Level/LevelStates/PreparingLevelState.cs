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
        yield return new WaitForSeconds(2f);
        LevelManager.SetState<InProgressLevelState>();
    }
}
