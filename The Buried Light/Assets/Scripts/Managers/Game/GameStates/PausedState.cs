using UnityEngine;

public class PausedState : GameStateBase
{
    public override void OnStateEnter(GameManager gameManager)
    {
        base.OnStateEnter(gameManager);
        Time.timeScale = 0;
        Debug.Log("Game is Paused.");
    }

    public override void OnStateExit()
    {
        Time.timeScale = 1;
        base.OnStateExit();
    }
}
