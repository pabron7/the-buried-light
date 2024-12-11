using UnityEngine;
using Cysharp.Threading.Tasks;

public class InProgressLevelState : LevelStateBase
{
    public override async UniTask OnStateEnterAsync(LevelManager levelManager)
    {
        await base.OnStateEnterAsync(levelManager);
        Debug.Log("LevelManager: InProgress state.");

        // Start the next phase asynchronously
        await LevelManager.StartNextPhaseAsync();
    }
}
