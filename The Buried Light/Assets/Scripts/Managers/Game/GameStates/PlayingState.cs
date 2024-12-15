using UnityEngine;

public class PlayingState : GameStateBase
{
    public override void OnStateEnter(GameManager gameManager, GameEvents gameEvents)
    {
        base.OnStateEnter(gameManager, gameEvents);
        Time.timeScale = 1;
        gameEvents.NotifyGameStarted();
        Debug.Log("Game is now Playing.");
    }

    public override void OnUpdate()
    {
    }
}

