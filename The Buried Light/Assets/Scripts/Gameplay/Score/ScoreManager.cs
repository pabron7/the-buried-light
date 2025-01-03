using UniRx;
using Zenject;
using UnityEngine;

public class ScoreManager
{
    private readonly ReactiveProperty<int> _currentScore = new ReactiveProperty<int>(0);
    public IReadOnlyReactiveProperty<int> CurrentScore => _currentScore;

    private readonly CompositeDisposable _disposables = new CompositeDisposable();

    private readonly ResetScore _resetScore;
    private readonly AddScore _addScore;

    [Inject]
    public ScoreManager(EnemyEvents enemyEvents, GameEvents gameEvents)
    {
        _resetScore = new ResetScore();
        _addScore = new AddScore();

        // Subscribe to OnEnemyScore events
        enemyEvents.OnEnemyScore
            .Subscribe(AddScore)
            .AddTo(_disposables);

        // Subscribe to OnGameStarted event to reset score
        gameEvents.OnGameStarted
            .Subscribe(_ => ResetScore())
            .AddTo(_disposables);
    }

    private void AddScore(IScoreGiver scoreGiver)
    {
        if (scoreGiver == null) return;

        _addScore.Execute(_currentScore, scoreGiver.ScoreValue);
    }

    private void ResetScore()
    {
        _resetScore.Execute(_currentScore);
    }

    // Ensure proper cleanup of subscriptions
    ~ScoreManager()
    {
        _disposables.Dispose();
        Debug.Log("ScoreManager disposed.");
    }
}

