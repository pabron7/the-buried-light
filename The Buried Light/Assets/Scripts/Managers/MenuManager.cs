using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class MenuManager : MonoBehaviour
{
   // [SerializeField] private GameObject mainMenuCanvas;
    [SerializeField] private Button startButton;        

    [Inject] private GameManager _gameManager;

    private void Awake()
    {
        // mainMenuCanvas.SetActive(true);

        // Attach button click event
        startButton.onClick.AddListener(OnStartButtonClicked);
    }

    private void OnStartButtonClicked()
    {
        // Transition the game to the Playing state
        _gameManager.SetState<PlayingState>();

        // Hide the main menu
       //  mainMenuCanvas.SetActive(false);
    }
}
