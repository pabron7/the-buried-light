using UnityEngine;
using Cysharp.Threading.Tasks;
using Zenject;

public class TitleScreenController : MonoBehaviour
{
    [Tooltip("The full title screen UI")]
    [SerializeField] private GameObject titleScreen;
    [Tooltip("The 'Press Anything to Continue' text")]
    [SerializeField] private GameObject pressText;

    [Inject] private GameManager _gameManager;

    private bool isWaitingForInput = false;

    private async void Awake()
    {
        titleScreen.SetActive(true);
        pressText.SetActive(false);

        // Wait for GameManager initialization
        await UniTask.WaitUntil(() => _gameManager != null);

        // Delay for 1 second and show the press text
        await UniTask.Delay(1000);
        ShowPressText();
    }

    /// <summary>
    /// Display the text. Allow key read.
    /// </summary>
    private void ShowPressText()
    {
        pressText.SetActive(true);
        isWaitingForInput = true;
    }

    private void Update()
    {
        if (isWaitingForInput && Input.anyKeyDown)
        {
            // Call async method instead of starting a coroutine
            HideTitleScreenAndContinue().Forget();
        }
    }

    /// <summary>
    /// Hide the title screen. Set the GameManager to MainMenu state
    /// </summary>
    private async UniTaskVoid HideTitleScreenAndContinue()
    {
        isWaitingForInput = false;

        // A small delay representing an animation or effect
        await UniTask.Delay(500);

        pressText.SetActive(false);
        titleScreen.SetActive(false);
        _gameManager.SetState<MainMenuState>();
    }
}
