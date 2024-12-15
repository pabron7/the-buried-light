using UnityEngine;

public class MainMenuState : GameStateBase
{
    public override void OnStateEnter(GameManager gameManager, GameEvents gameEvents)
    {
        base.OnStateEnter(gameManager, gameEvents);
        Time.timeScale = 0;
        gameEvents.NotifyMainMenu();
        Debug.Log("Game is in Main Menu.");
    }

    public override void OnStateExit()
    {
        Time.timeScale = 1;
        base.OnStateExit();
    }
}

