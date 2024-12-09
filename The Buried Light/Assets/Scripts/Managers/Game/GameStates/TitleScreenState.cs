using UnityEngine;

public class TitleScreenState : GameStateBase
{
    public override void OnStateEnter(GameManager gameManager)
    {
        base.OnStateEnter(gameManager);
        Time.timeScale = 1;
        Debug.Log("Game is in Title Screen State.");
    }

    public override void OnStateExit()
    {
        Time.timeScale = 0;
        base.OnStateExit();
    }
}
