using UnityEngine;

public abstract class GameStateBase
{
    protected GameManager GameManager;
    protected GameEvents GameEvents;

    public virtual void OnStateEnter(GameManager gameManager, GameEvents gameEvents)
    {
        GameManager = gameManager;
        GameEvents = gameEvents;
        Debug.Log($"{GetType().Name} Entered.");
    }

    public virtual void OnStateExit()
    {
        Debug.Log($"{GetType().Name} Exited.");
    }

    public virtual void OnUpdate() { }
}
