using UnityEngine;
using Zenject;
using TMPro;
using UniRx;
using Cysharp.Threading.Tasks;
using DG.Tweening;

public class ScoreUI : MonoBehaviour
{
    private LazyInject<ScoreManager> _scoreManager;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private float animationDuration = 0.2f;
    [SerializeField] private float scaleRatio = 1.2f;

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

        // Bind the score to the UI with animation
        _scoreManager.Value.CurrentScore.Subscribe(score =>
            {
                scoreText.text = $"Score: {score}";
                AnimateScoreChange();
            }).AddTo(this);
    }

    // Make sure score manager is initialized
    private async UniTask WaitForLazyInjection<T>(LazyInject<T> lazyInject) where T : class
    {
        await UniTask.WaitUntil(() => lazyInject.Value != null);
    }

    /// <summary>
    /// Animates the score text when it changes.
    /// </summary>
    private void AnimateScoreChange()
    {
        scoreText.transform.DOScale(scaleRatio, animationDuration / 2)
            .SetEase(Ease.OutQuad)
            .OnComplete(() =>
            {
                scoreText.transform.DOScale(1f, animationDuration / 2).SetEase(Ease.InQuad);
            });
    }
}
