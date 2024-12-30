using UniRx;
using Zenject;
using UnityEngine;

public class ScoreManager
{
    private readonly ReactiveProperty<int> _currentScore = new ReactiveProperty<int>(0);
    public IReadOnlyReactiveProperty<int> CurrentScore => _currentScore;

    private readonly CompositeDisposable _disposables = new CompositeDisposable();

    [Inject]
    public ScoreManager(EnemyEvents enemyEvents)
    {
        // Subscribe to OnEnemyScore events
        enemyEvents.OnEnemyScore
            .Subscribe(AddScore)
            .AddTo(_disposables);
    }

    private void AddScore(IScoreGiver scoreGiver)
    {
        if (scoreGiver == null) return;

        _currentScore.Value += scoreGiver.ScoreValue;
    }

    public void ResetScore()
    {
        _currentScore.Value = 0;
        Debug.Log("Score Reset.");
    }

    // Ensure proper cleanup of subscriptions
    ~ScoreManager()
    {
        _disposables.Dispose();
        Debug.Log("ScoreManager disposed.");
    }
}
