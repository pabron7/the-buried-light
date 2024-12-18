using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class MainMenuButton : MonoBehaviour
{
    private GameManager _gameManager;

    [Inject]
    public void Construct(GameManager gameManager)
    {
        _gameManager = gameManager;
    }

    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(SetToMainMenu);
    }

    private void SetToMainMenu()
    {
        if (_gameManager.CurrentState.Value is MainMenuState)
        {
            Debug.LogWarning("Game is already in Main Menu.");
            return;
        }

        _gameManager.SetState<MainMenuState>();
        Debug.Log("Game state set to Main Menu.");
    }
}