using UnityEngine;
using UniRx;
using Zenject;
using System.Collections;
using TMPro;

public class LevelStartNotifyer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI indicatorText; 
    [SerializeField] private float displayDuration = 2f; 

    [Inject] private GameEvents _gameEvents;

    private void Start()
    {
        // Subscribe to level and phase events
        _gameEvents.OnLevelStart.Subscribe(_ => ShowIndicator("Level Starts")).AddTo(this);
        _gameEvents.OnPhaseStart.Subscribe(phase => ShowIndicator($"Phase {phase } Started")).AddTo(this);
        _gameEvents.OnPhaseEnd.Subscribe(phase => ShowIndicator($"Phase {phase } Completed")).AddTo(this);
        _gameEvents.OnLevelEnd.Subscribe(_ => ShowIndicator("Congratulations!")).AddTo(this);
    }

    private void ShowIndicator(string message)
    {
        StopAllCoroutines(); // Stop any existing coroutine
        StartCoroutine(DisplayMessage(message));
    }

    private IEnumerator DisplayMessage(string message)
    {
        indicatorText.text = message;
        indicatorText.gameObject.SetActive(true);

        yield return new WaitForSeconds(displayDuration);

        indicatorText.gameObject.SetActive(false);
    }
}
