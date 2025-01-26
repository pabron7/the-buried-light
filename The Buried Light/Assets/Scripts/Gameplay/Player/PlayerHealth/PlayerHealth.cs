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

    private HealPlayer _healPlayer;
    private DamagePlayer _damagePlayer;

    private void Awake()
    {
        // Initialize ReactiveProperty
        CurrentHealth = new ReactiveProperty<int>(maxHealth);

        // Initialize the helper classes
        _healPlayer = new HealPlayer(CurrentHealth, maxHealth);
        _damagePlayer = new DamagePlayer(CurrentHealth, maxHealth, _playerEvents);
    }

    private void Start()
    {
        // Ensure Health starts at Max Health
        CurrentHealth.Value = maxHealth;
    }

    /// <summary>
    /// Delegates healing to HealPlayer.
    /// </summary>
    /// <param name="amount">The amount of health to restore.</param>
    public void Heal(int amount)
    {
        _healPlayer.Heal(amount);
    }

    /// <summary>
    /// Delegates damage handling to DamagePlayer.
    /// </summary>
    /// <param name="damage">The amount of damage to take.</param>
    public void TakeDamage(int damage)
    {
        _damagePlayer.Damage(damage);
    }

}