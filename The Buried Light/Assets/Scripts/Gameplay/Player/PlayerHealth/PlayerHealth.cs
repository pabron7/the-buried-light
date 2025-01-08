using UnityEngine;
using Zenject;

public class PlayerHealth : MonoBehaviour, IHealth
{
    [SerializeField] private int maxHealth = 2;
    private int _currentHealth;

    public int CurrentHealth => _currentHealth;
    [Inject] PlayerEvents _playerEvents;

    private void Start()
    {
        _currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        _currentHealth = Mathf.Max(_currentHealth - damage, 0);
        Debug.Log($"Player took {damage} damage. Remaining health: {_currentHealth}");

        if (_currentHealth <= 0)
        {
            Debug.Log("Player has died!");
            _playerEvents.NotifyPlayerDeath();
            _currentHealth = maxHealth; // Reset player health
        }
    }

    public void Heal(int amount)
    {
        _currentHealth = Mathf.Min(_currentHealth + amount, maxHealth);
        Debug.Log($"Player healed by {amount}. Current health: {_currentHealth}");
    }
}
