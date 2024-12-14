using UnityEngine;

public abstract class LevelStateBase
{
    protected LevelManager LevelManager;

    public virtual void OnStateEnter(LevelManager levelManager)
    {
        LevelManager = levelManager;
        Debug.Log($"{GetType().Name} Entered.");
    }

    public virtual void OnStateExit()
    {
        Debug.Log($"{GetType().Name} Exited.");
    }

    public virtual void OnUpdate()
    {
    }
}

