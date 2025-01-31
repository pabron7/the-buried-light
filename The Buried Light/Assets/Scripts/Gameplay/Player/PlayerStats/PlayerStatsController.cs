using UnityEngine;
using UniRx;

public class PlayerStatsController
{
    // Reactive Properties for automatic UI updates
    public ReactiveProperty<int> MaxHealth { get; private set; }
    public ReactiveProperty<int> CurrentHealth { get; private set; }
    public ReactiveProperty<float> MovementSpeed { get; private set; }
    public ReactiveProperty<int> Damage { get; private set; }

    /// <summary>
    /// Base stats declared inside the class instead of constructor parameters
    /// </summary>
    public PlayerStatsController()
    {
        // Default base stats (Modify these values as needed)
        int baseHealth = 100;
        float baseSpeed = 5.0f;
        int baseDamage = 10;

        // Initialize reactive properties with base values
        MaxHealth = new ReactiveProperty<int>(baseHealth);
        CurrentHealth = new ReactiveProperty<int>(baseHealth);
        MovementSpeed = new ReactiveProperty<float>(baseSpeed);
        Damage = new ReactiveProperty<int>(baseDamage);
    }

    /// <summary>
    /// Adjusts the player's health by the given amount.
    /// </summary>
    public void ModifyHealth(int amount)
    {
        CurrentHealth.Value = Mathf.Clamp(CurrentHealth.Value + amount, 0, MaxHealth.Value);
    }

    /// <summary>
    /// Adjusts the player's damage by the given amount.
    /// </summary>
    public void ModifyDamage(int amount)
    {
        Damage.Value += amount;
    }

    /// <summary>
    /// Adjusts the player's movement speed.
    /// </summary>
    public void ModifySpeed(float amount)
    {
        MovementSpeed.Value += amount;
    }

    /// <summary>
    /// Resets stats to default base values.
    /// </summary>
    public void ResetStats()
    {
        CurrentHealth.Value = MaxHealth.Value;
        MovementSpeed.Value = 5.0f;
        Damage.Value = 10;
    }
}
