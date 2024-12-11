using System;

public interface IKillable
{
    int Health { get; }
    void TakeDamage(int damage);
    void OnDeath();

}
