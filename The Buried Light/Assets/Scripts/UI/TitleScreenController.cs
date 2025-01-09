using UnityEngine;
using Cysharp.Threading.Tasks;
using Zenject;
using UniRx;

public class TitleScreenController : MonoBehaviour
{
    [Tooltip("The full title screen UI")]
    [SerializeField] private GameObject titleScreen;
    [Tooltip("The 'Press Anything to Continue' text")]
    [SerializeField] private GameObject pressText;

    [Inject] private GameManager _gameManager;
    [Inject] private GameEvents _gameEvents;

    private bool _isWaitingForInput;

    private void Awake()
    {
        ShowTitleScreen();
    }

    private void Start()
    {
        // Ensure UI elements are initially hidden
        titleScreen.SetActive(true);
        pressText.SetActive(false);
    }

    private void ShowTitleScreen()
    {
        titleScreen.SetActive(true);
        pressText.SetActive(false);
        _isWaitingForInput = false;

        ShowPressText().Forget();
    }

    private async UniTaskVoid ShowPressText()
    {
        // Delay for a short duration before showing "Press Any Key"
        await UniTask.Delay(1000);
        pressText.SetActive(true);
        _isWaitingForInput = true;
    }

    private void Update()
    {
        if (_isWaitingForInput && Input.anyKeyDown)
        {
            HideTitleScreenAndContinue().Forget();
        }
    }

    private async UniTaskVoid HideTitleScreenAndContinue()
    {
        Debug.Log("Hiding Title Screen and transitioning to Main Menu");
        _isWaitingForInput = false;

        // Perform some transition/animation delay if needed
        // await UniTask.Delay(500);

        pressText.SetActive(false);
        titleScreen.SetActive(false);

        // Transition to the Main Menu state
        _gameManager.SetState<MainMenuState>();
    }
}
