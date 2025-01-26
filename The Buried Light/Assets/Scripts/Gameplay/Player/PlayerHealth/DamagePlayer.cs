using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class DamagePlayer
{
    private readonly ReactiveProperty<int> _currentHealth;
    private readonly int _maxHealth;
    private readonly PlayerEvents _playerEvents;

    public DamagePlayer(ReactiveProperty<int> currentHealth, int maxHealth, PlayerEvents playerEvents)
    {
        _currentHealth = currentHealth;
        _maxHealth = maxHealth;
        _playerEvents = playerEvents;
    }

    /// <summary>
    /// Damages the player by the specified amount, ensuring health doesn't drop below zero.
    /// </summary>
    /// <param name="damage">The amount of damage to take.</param>
    public void Damage(int damage)
    {
        _currentHealth.Value = Mathf.Max(_currentHealth.Value - damage, 0);

        if (_currentHealth.Value <= 0)
        {
            Debug.Log("Player has died!");
            _playerEvents.NotifyPlayerDeath();
        }
    }
}

