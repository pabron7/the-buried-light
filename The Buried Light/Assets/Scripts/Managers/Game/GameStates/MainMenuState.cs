using UnityEngine;
using Zenject;
using Cysharp.Threading.Tasks;

public class MainMenuState : GameStateBase
{
    [Inject] private SceneManager _sceneManager;

    public override async void OnStateEnter(GameManager gameManager, GameEvents gameEvents)
    {
        // Load the MainMenu scene and wait for it to complete
        await _sceneManager.LoadSceneAsync("MainMenu");
        await UniTask.Delay(1000);

        base.OnStateEnter(gameManager, gameEvents);

        Debug.Log("Entering MainMenuState...");

        // Notify that the Main Menu is active
        gameEvents.NotifyMainMenu();

        Time.timeScale = 0; 
        Debug.Log("Game is in Main Menu.");
    }

    public override void OnStateExit()
    {
        Debug.Log("Exiting MainMenuState...");
        base.OnStateExit();

        Time.timeScale = 1;
    }
}


