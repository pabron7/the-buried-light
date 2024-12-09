using UnityEngine;

public abstract class GameStateBase
{
    protected GameManager GameManager;

    public virtual void OnStateEnter(GameManager gameManager)
    {
        GameManager = gameManager;
        Debug.Log($"{GetType().Name} Entered.");
    }

    public virtual void OnStateExit()
    {
        Debug.Log($"{GetType().Name} Exited.");
    }

    public virtual void OnUpdate() { }
}
