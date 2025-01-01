using UnityEngine;
using Cysharp.Threading.Tasks;
using Zenject;

public class PlayingState : GameStateBase
{
    [Inject] private SceneManager _sceneManager;

    public override async void OnStateEnter(GameManager gameManager, GameEvents gameEvents)
    {
        base.OnStateEnter(gameManager, gameEvents);

        // Load the Gameplay scene and wait for it to complete
        await _sceneManager.LoadSceneAsync("Gameplay");

        // Notify that the game has started
        gameEvents.NotifyGameStarted();
        await UniTask.Delay(1000);

        Debug.Log("PlayingState: Scene loaded. Preparing to start the level...");
        await UniTask.Delay(3000);
  
        Time.timeScale = 1;

        // Notify that the level has started
        gameEvents.NotifyLevelStart();

        Debug.Log("PlayingState: Level started.");
    }

    public override void OnUpdate()
    {
    }
}
