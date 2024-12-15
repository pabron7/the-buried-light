using UnityEngine;

public class GameOverState : GameStateBase
{
    public override void OnStateEnter(GameManager gameManager, GameEvents gameEvents)
    {
        base.OnStateEnter(gameManager, gameEvents);
        Time.timeScale = 0;
        gameEvents.NotifyGameOver();
        Debug.Log("Game Over.");
    }
}