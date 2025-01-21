using UnityEngine;
using UnityEngine.UI;
using Zenject;
using TMPro;

public class PauseButton : MonoBehaviour
{
    private GameManager _gameManager;
    private TextMeshProUGUI buttonText;
  

    [Inject]
    public void Construct(GameManager gameManager)
    {
        _gameManager = gameManager;
    }

    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(TogglePause);
        buttonText = GetComponentInChildren<TextMeshProUGUI>();
    }

    private void TogglePause()
    {
        if (_gameManager.CurrentState.Value is PausedState)
        {
            buttonText.text = "Resume";
            _gameManager.SetState<PlayingState>();
        }
        else if (_gameManager.CurrentState.Value is PlayingState)
        {
            buttonText.text = "Pause";
            _gameManager.SetState<PausedState>();
        }
        else
        {
            Debug.LogWarning("Pause button clicked, but the game is not in a state that can be paused or resumed.");
        }
    }
}
