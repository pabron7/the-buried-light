using UnityEngine;

public class TitleScreenState : GameStateBase
{
    public override void OnStateEnter(GameManager gameManager, GameEvents gameEvents)
    {
        base.OnStateEnter(gameManager, gameEvents);
        Time.timeScale = 1;
        gameEvents.NotifyTitleScreen();
    }

    public override void OnStateExit()
    {
        // Time.timeScale = 1; 
        base.OnStateExit();
    }
}
