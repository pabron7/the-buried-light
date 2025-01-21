using UnityEngine;
using Zenject;
using TMPro;
using System.Collections;
using UniRx;
using Cysharp.Threading.Tasks;

public class LevelStartNotifyer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI indicatorText;
    [SerializeField] private float displayDuration = 2f;

    private LazyInject<GameEvents> _gameEvents;

    [Inject]
    public void Construct(LazyInject<GameEvents> gameEvents)
    {
        _gameEvents = gameEvents;
    }

    private async void Start()
    {
        await WaitForLazyInjection(_gameEvents);

        if (_gameEvents.Value == null)
        {
            Debug.LogError("GameEvents dependency is not resolved.");
            return;
        }

        // Subscribe to level and phase events
        _gameEvents.Value.OnLevelStart
            .Subscribe(_ => ShowIndicator("Level Starts"))
            .AddTo(this);

        _gameEvents.Value.OnPhaseStart
            .Subscribe(phase => ShowIndicator($"Phase {phase} Started"))
            .AddTo(this);

        _gameEvents.Value.OnPhaseEnd
            .Subscribe(phase => ShowIndicator($"Phase {phase} Completed"))
            .AddTo(this);

        _gameEvents.Value.OnLevelEnd
            .Subscribe(_ => ShowIndicator("Congratulations!"))
            .AddTo(this);
    }

    private async UniTask WaitForLazyInjection<T>(LazyInject<T> lazyInject) where T : class
    {
        await UniTask.WaitUntil(() => lazyInject.Value != null);
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
