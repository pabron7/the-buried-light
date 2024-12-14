using UnityEngine;
using System;

public interface IEnemyEvents
{
    IObservable<Vector3> OnEnemyKilled { get; }
    IObservable<int> OnEnemyDamaged { get; }

    void NotifyEnemyKilled(Vector3 position);
    void NotifyEnemyDamaged(int damage);
}