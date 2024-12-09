using UnityEngine;

public class MainMenuState : GameStateBase
{
    public override void OnStateEnter(GameManager gameManager)
    {
        base.OnStateEnter(gameManager);
        Time.timeScale = 0;
        Debug.Log("Game is in Main Menu.");
    }

    public override void OnStateExit()
    {
        Time.timeScale = 1;
        base.OnStateExit();
    }
}
