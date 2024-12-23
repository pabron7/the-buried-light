using UniRx;
using UnityEngine;
using System;

public class EnemyEvents : IEnemyEvents
{
    private readonly Subject<Vector3> _onEnemyKilled = new Subject<Vector3>();
    private readonly Subject<int> _onEnemyDamaged = new Subject<int>();
    private readonly Subject<IScoreGiver> _onEnemyScore = new Subject<IScoreGiver>();

    public IObservable<Vector3> OnEnemyKilled => _onEnemyKilled;
    public IObservable<int> OnEnemyDamaged => _onEnemyDamaged;
    public IObservable<IScoreGiver> OnEnemyScore => _onEnemyScore;


    public void NotifyEnemyKilled(Vector3 position)
    {
        Debug.Log($"Enemy killed at position: {position}");
        _onEnemyKilled.OnNext(position);
    }

    public void NotifyEnemyDamaged(int damage)
    {
        Debug.Log($"Enemy damaged: {damage}");
        _onEnemyDamaged.OnNext(damage);
    }

    public void NotifyEnemyScore(IScoreGiver scoreGiver)
    {
        Debug.Log($"Enemy scored: {scoreGiver.ScoreValue}");
        _onEnemyScore.OnNext(scoreGiver);
    }
}
