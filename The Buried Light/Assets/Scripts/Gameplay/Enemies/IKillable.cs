using System;

public interface IKillable
{
    int Health { get; }
    void TakeDamage(int damage);
    void OnDeath();

    // New event for enemy death
    event Action<IKillable> EnemyDeath;
}
