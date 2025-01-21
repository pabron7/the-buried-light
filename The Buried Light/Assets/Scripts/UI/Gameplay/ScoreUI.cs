using UnityEngine;
using Zenject;
using TMPro;
using UniRx;
using Cysharp.Threading.Tasks;

public class ScoreUI : MonoBehaviour
{
    private LazyInject<ScoreManager> _scoreManager;
    [SerializeField] private TextMeshProUGUI scoreText;

    [Inject]
    public void Construct(LazyInject<ScoreManager> scoreManager)
    {
        _scoreManager = scoreManager;
    }

    private async void Start()
    {
        await WaitForLazyInjection(_scoreManager);

        if (_scoreManager.Value == null)
        {
            Debug.LogError("ScoreManager dependency is not resolved.");
            return;
        }

        // Bind the score to the UI
        _scoreManager.Value.CurrentScore
            .Subscribe(score => scoreText.text = $"Score: {score}")
            .AddTo(this);
    }

    private async UniTask WaitForLazyInjection<T>(LazyInject<T> lazyInject) where T : class
    {
        await UniTask.WaitUntil(() => lazyInject.Value != null);
    }
}
