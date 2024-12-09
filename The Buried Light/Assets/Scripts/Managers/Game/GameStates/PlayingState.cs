using UnityEngine;

public class PlayingState : GameStateBase
{
    public override void OnStateEnter(GameManager gameManager)
    {
        base.OnStateEnter(gameManager);
        Time.timeScale = 1;
        Debug.Log("Game is now Playing.");
    }

    public override void OnUpdate()
    {
    }
}
