using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class HealPlayer
{
    private readonly ReactiveProperty<int> _currentHealth;
    private readonly int _maxHealth;

    public HealPlayer(ReactiveProperty<int> currentHealth, int maxHealth)
    {
        _currentHealth = currentHealth;
        _maxHealth = maxHealth;
    }

    /// <summary>
    /// Heals the player by the specified amount, ensuring it doesn't exceed the maximum health.
    /// </summary>
    /// <param name="amount">The amount of health to restore.</param>
    public void Heal(int amount)
    {
        _currentHealth.Value = Mathf.Min(_currentHealth.Value + amount, _maxHealth);
    }
}

