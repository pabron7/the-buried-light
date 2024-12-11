using UniRx;
using UnityEngine;

public class EnemyEvents
{
    public Subject<Vector3> OnEnemyKilled { get; } = new Subject<Vector3>();
    public Subject<int> OnEnemyDamaged { get; } = new Subject<int>();

    public void NotifyEnemyKilled(Vector3 position)
    {
        Debug.Log($"Enemy killed at position: {position}");
        OnEnemyKilled.OnNext(position);
    }

    public void NotifyEnemyDamaged(int damage)
    {
        Debug.Log($"Enemy damaged: {damage}");
        OnEnemyDamaged.OnNext(damage);
    }
}
