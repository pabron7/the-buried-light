using UnityEngine;
using UniRx;
using Zenject;
using System.Collections;

public class TitleScreenController : MonoBehaviour
{
    [Tooltip("The full title screen UI")]
    [SerializeField] private GameObject titleScreen;
    [Tooltip("The 'Press Anything to Continue' text")]
    [SerializeField] private GameObject pressText;

    [Inject] private GameManager _gameManager;

    private bool isWaitingForInput = false;

    private void Awake()
    {
        titleScreen.SetActive(true);
        pressText.SetActive(false);

        // Wait for GameManager initialization before proceeding
        Observable.EveryUpdate()
            .Where(_ => _gameManager != null)
            .First()
            .Subscribe(_ =>
            {
                Observable.Timer(System.TimeSpan.FromSeconds(1))
                    .Subscribe(__ => ShowPressText())
                    .AddTo(this);
            })
            .AddTo(this);
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
            StartCoroutine(HideTitleScreenAndContinue());
        }
    }

    /// <summary>
    /// Hide the title screen. Set the GameManager to MainMenu state
    /// </summary>
    /// <returns></returns>
    private IEnumerator HideTitleScreenAndContinue()
    {
        isWaitingForInput = false;


        // A small delay representing an animation or effect
        yield return new WaitForSeconds(0.5f);

        pressText.SetActive(false);
        titleScreen.SetActive(false);
        _gameManager.SetState<MainMenuState>();
    }
}
