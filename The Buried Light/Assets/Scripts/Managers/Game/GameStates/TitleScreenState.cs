using UnityEngine;
using Zenject;
using Cysharp.Threading.Tasks;

public class TitleScreenState : GameStateBase
{
    [Inject] private SceneManager _sceneManager;

    public override async void OnStateEnter(GameManager gameManager, GameEvents gameEvents)
    {
        // Load the TitleScreen scene and wait for it to complete
        await _sceneManager.LoadSceneAsync("TitleScreen");

        base.OnStateEnter(gameManager, gameEvents);

        Debug.Log("Entering TitleScreenState...");

        // Notify that the Title Screen is active
        gameEvents.NotifyTitleScreen();

        Time.timeScale = 0; 
    }

    public override void OnStateExit()
    {
        Debug.Log("Exiting TitleScreenState...");
        base.OnStateExit();
    }
}
