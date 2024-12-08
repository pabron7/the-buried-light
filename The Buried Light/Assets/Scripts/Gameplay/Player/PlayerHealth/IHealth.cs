public interface IHealth
{
    int CurrentHealth { get; }
    void TakeDamage(int damage);
    void Heal(int amount);
    event System.Action<int> OnHealthChanged;
    event System.Action OnDeath;
}
