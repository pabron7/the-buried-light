using UnityEngine;
using Cysharp.Threading.Tasks;

public class IdleLevelState : LevelStateBase
{
    public override async UniTask OnStateEnterAsync(LevelManager levelManager)
    {
        await base.OnStateEnterAsync(levelManager);
        Debug.Log("Level is idle. Ready to initialize or start.");

        // Reset WaveManager pool and prepare for the next level or phase
        LevelManager.ResetWavePool();
        await UniTask.Yield();
    }
}
