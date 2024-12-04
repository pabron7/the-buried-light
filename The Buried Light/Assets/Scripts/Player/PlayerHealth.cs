using UnityEngine;

public class PlayerHealth : MonoBehaviour, IHealth
{
    [SerializeField] private int maxHealth = 10;
    private int _currentHealth;

    public int CurrentHealth => _currentHealth;

    public event System.Action<int> OnHealthChanged;
    public event System.Action OnDeath;

    private void Start()
    {
        _currentHealth = maxHealth;
        OnHealthChanged?.Invoke(_currentHealth);
    }

    public void TakeDamage(int damage)
    {
        _currentHealth = Mathf.Max(_currentHealth - damage, 0);
        Debug.Log($"Player took {damage} damage. Remaining health: {_currentHealth}");
        OnHealthChanged?.Invoke(_currentHealth);

        if (_currentHealth <= 0)
        {
            OnDeath?.Invoke();
            Debug.Log("Player has died!");
        }
    }

    public void Heal(int amount)
    {
        _currentHealth = Mathf.Min(_currentHealth + amount, maxHealth);
        Debug.Log($"Player healed by {amount}. Current health: {_currentHealth}");
        OnHealthChanged?.Invoke(_currentHealth);
    }
}
