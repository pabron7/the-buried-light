using UnityEngine;
using Cysharp.Threading.Tasks;

public abstract class LevelStateBase : MonoBehaviour
{
    protected LevelManager LevelManager;

    /// <summary>
    /// Asynchronously enters the state.
    /// </summary>
    public virtual async UniTask OnStateEnterAsync(LevelManager levelManager)
    {
        LevelManager = levelManager;
        Debug.Log($"{GetType().Name} Entered.");
        await UniTask.Yield(); // Allow for any async logic in derived classes
    }

    /// <summary>
    /// Asynchronously exits the state.
    /// </summary>
    public virtual async UniTask OnStateExitAsync()
    {
        Debug.Log($"{GetType().Name} Exited.");
        await UniTask.Yield(); // Allow for any async logic in derived classes
    }

    /// <summary>
    /// Per-frame update logic.
    /// </summary>
    public virtual void OnUpdate()
    {
    }
}
