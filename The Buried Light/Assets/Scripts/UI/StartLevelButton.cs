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
        _gameManager.SetState<PlayingState>();
    }
}