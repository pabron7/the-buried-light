using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class ResumeGameButton : MonoBehaviour
{
    private GameManager _gameManager;

    [Inject]
    public void Construct(GameManager gameManager)
    {
        _gameManager = gameManager;
    }

    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(ResumeGame);
    }

    private void ResumeGame()
    {
        if (_gameManager.CurrentState.Value is PlayingState)
        {
            Debug.LogWarning("Game is already playing.");
            return;
        }

        _gameManager.SetState<PlayingState>();
        Debug.Log("Game state set to Playing.");
    }
}