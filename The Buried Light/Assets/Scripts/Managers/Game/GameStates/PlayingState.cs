using UnityEngine;
using Cysharp.Threading.Tasks;

public class PlayingState : GameStateBase
{
    public override async void OnStateEnter(GameManager gameManager, GameEvents gameEvents)
    {
        base.OnStateEnter(gameManager, gameEvents);
        Time.timeScale = 1;
        gameEvents.NotifyGameStarted();

        Debug.Log("Game is now Playing. Preparing to start the level...");

        // Add a short delay before starting the level
        await UniTask.Delay(2000); // 2-second delay
        gameEvents.NotifyLevelStart();

        Debug.Log("PlayingState: Level started.");
    }

    public override void OnUpdate()
    {

    }
}