using UnityEngine;

public class PausedState : GameStateBase
{
    public override void OnStateEnter(GameManager gameManager, GameEvents gameEvents)
    {
        base.OnStateEnter(gameManager, gameEvents);
        Time.timeScale = 0;
        gameEvents.NotifyPaused();
        Debug.Log("Game is Paused.");
    }

    public override void OnStateExit()
    {
        Time.timeScale = 1;
        base.OnStateExit();
    }
}

