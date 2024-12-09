using UnityEngine;

public class GameOverState : GameStateBase
{
    public override void OnStateEnter(GameManager gameManager)
    {
        base.OnStateEnter(gameManager);
        Time.timeScale = 0;
        Debug.Log("Game Over.");
    }
}
