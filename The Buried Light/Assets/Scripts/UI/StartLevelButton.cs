using UnityEngine;
using Zenject;
using UnityEngine.UI;

public class StartLevelButton : MonoBehaviour
{
    private Button startButton;

    [Inject] private GameManager _gameManager;

    private void Awake()
    {
        startButton = GetComponent<Button>();
        if (startButton == null)
        {
            Debug.LogError("StartLevelButton: Button component not found!");
            return;
        }

        startButton.onClick.AddListener(OnStartButtonClicked);
    }



    private void OnStartButtonClicked()
    {
        var gameManager = FindObjectOfType<GameManager>();
        if (gameManager == null)
        {
            Debug.LogError("GameManager not found in the scene or ProjectContext.");
            return;
        }

        gameManager.SetState<PlayingState>();
    }

}