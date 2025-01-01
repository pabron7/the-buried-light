using UnityEngine;
using UniRx;
using Zenject;

public class GameplayUIController : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;   
    [SerializeField] private GameObject gameOverMenu; 

    private GameEvents _gameEvents;

    [Inject]
    public void Construct(GameEvents gameEvents)
    {
        _gameEvents = gameEvents;
    }

    private void Start()
    {
        // Subscribe to GameEvents
        _gameEvents.OnPaused
            .Subscribe(_ => ShowPauseMenu())
            .AddTo(this);

        _gameEvents.OnResumed
            .Subscribe(_ => HidePauseMenu())
            .AddTo(this);

        _gameEvents.OnGameOver
            .Subscribe(_ => ShowGameOverMenu())
            .AddTo(this);
    }

    private void ShowPauseMenu()
    {
        if (pauseMenu != null)
        {
            pauseMenu.SetActive(true);
            Debug.Log("Pause Menu Activated.");
        }
    }

    private void HidePauseMenu()
    {
        if (pauseMenu != null)
        {
            pauseMenu.SetActive(false);
            Debug.Log("Pause Menu Deactivated.");
        }
    }

    private void ShowGameOverMenu()
    {
        if (gameOverMenu != null)
        {
            gameOverMenu.SetActive(true);
            Debug.Log("GameOver Menu Activated.");
        }
    }
}
