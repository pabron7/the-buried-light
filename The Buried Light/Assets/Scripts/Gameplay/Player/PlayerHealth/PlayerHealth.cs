using UnityEngine;
using UniRx;
using Zenject;

public class PlayerHealth : MonoBehaviour, IHealth
{
    [SerializeField] private int maxHealth = 2;

    /// <summary>
    /// Reactive property to monitor current health changes.
    /// </summary>
    public ReactiveProperty<int> CurrentHealth { get; private set; }

    [Inject] private PlayerEvents _playerEvents;

    private void Awake()
    {
        CurrentHealth = new ReactiveProperty<int>(maxHealth);
    }

    private void Start()
    {
        // Make sure Health is equal to Max Health at the start
        CurrentHealth.Value = maxHealth;
    }

    /// <summary>
    /// Reduces the player's health by the specified damage amount.
    /// </summary>
    /// <param name="damage">The amount of damage to take.</param>
    public void TakeDamage(int damage)
    {
        CurrentHealth.Value = Mathf.Max(CurrentHealth.Value - damage, 0);
        Debug.Log($"Player took {damage} damage. Remaining health: {CurrentHealth.Value}");

        if (CurrentHealth.Value <= 0)
        {
            Debug.Log("Player has died!");
            _playerEvents.NotifyPlayerDeath();
            ResetHealth();
        }
    }

    /// <summary>
    /// Increases the player's health by the specified amount.
    /// </summary>
    /// <param name="amount">The amount of health to restore.</param>
    public void Heal(int amount)
    {
        CurrentHealth.Value = Mathf.Min(CurrentHealth.Value + amount, maxHealth);
        Debug.Log($"Player healed by {amount}. Current health: {CurrentHealth.Value}");
    }

    /// <summary>
    /// Resets the player's health to the maximum value.
    /// </summary>
    private void ResetHealth()
    {
        CurrentHealth.Value = maxHealth;
    }
}

