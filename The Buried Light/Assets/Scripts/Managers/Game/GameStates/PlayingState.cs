using UnityEngine;
using Cysharp.Threading.Tasks;
using Zenject;

public class PlayingState : GameStateBase
{
    [Inject] private SceneManager _sceneManager;

    public override async void OnStateEnter(GameManager gameManager, GameEvents gameEvents)
    {
        base.OnStateEnter(gameManager, gameEvents);

        // Check if the previous state was PauseState
        if (gameManager.PreviousState is PausedState)
        {
            Debug.Log("Resuming game from pause. No scene reload necessary.");
            Time.timeScale = 1;
            gameEvents.NotifyResumed();
            return; // Skip loading the scene
        }

        // Load the Gameplay scene and wait for it to complete
        Debug.Log("Loading Gameplay Scene.");
        await _sceneManager.LoadSceneAsync("Gameplay");

        // Notify that the game has started
        gameEvents.NotifyGameStarted();

        Time.timeScale = 1;
    }
}

