using UnityEngine;
using Cysharp.Threading.Tasks;

public class PreparingLevelState : LevelStateBase
{
    public override async UniTask OnStateEnterAsync(LevelManager levelManager)
    {
        await base.OnStateEnterAsync(levelManager);
        Debug.Log("Preparing level...");

        // Simulate preparation work asynchronously
        await PrepareLevelAsync();
    }

    private async UniTask PrepareLevelAsync()
    {
        // Simulate a 2-second preparation delay
        await UniTask.Delay(2000);

        // Transition to the in-progress state to start waves
        await LevelManager.SetStateAsync<InProgressLevelState>();
    }
}
