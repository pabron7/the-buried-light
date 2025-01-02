using UnityEngine;
using Cysharp.Threading.Tasks;

/// <summary>
/// Manages the health, damage, and death logic for an enemy.
/// </summary>
public class EnemyHealth : MonoBehaviour
{
    public int CurrentHealth { get; private set; }

    private bool _isSpawner;
    private EnemyTypes _spawnType;
    private int _spawnCount;
    private int _spawnHealth;
    private float _spawnSpeed;

    private EnemySpawner _enemySpawner;
    private EnemyEvents _enemyEvents;

    private System.Action _onDamageTaken; // Callback for visual effects or other damage behaviors
    private System.Action _onDeathCallback; // Callback for orchestrating death behavior

    /// <summary>
    /// Initializes the health of the enemy.
    /// </summary>
    public void Initialize(int health, EnemySpawner enemySpawner, EnemyEvents enemyEvents, System.Action onDamageTaken, System.Action onDeathCallback)
    {
        CurrentHealth = health;
        _enemySpawner = enemySpawner;
        _enemyEvents = enemyEvents;
        _onDamageTaken = onDamageTaken;
        _onDeathCallback = onDeathCallback;
    }

    /// <summary>
    /// Configures on-death spawning behavior.
    /// </summary>
    public void ConfigureOnDeathSpawn(EnemyTypes spawnType, int spawnCount, int spawnHealth, float spawnSpeed)
    {
        _isSpawner = true;
        _spawnType = spawnType;
        _spawnCount = spawnCount;
        _spawnHealth = spawnHealth;
        _spawnSpeed = spawnSpeed;
    }

    /// <summary>
    /// Applies damage to the enemy.
    /// </summary>
    public void TakeDamage(int damage)
    {
        if (damage <= 0)
        {
            return;
        }

        CurrentHealth -= damage;

        // Trigger visual effects or behaviors tied to damage
        _onDamageTaken?.Invoke();

        // Notify damage event
        _enemyEvents.NotifyEnemyDamaged(damage);

        if (CurrentHealth <= 0)
        {
            OnDeath().Forget(); // Fire-and-forget death handling
        }
    }

    /// <summary>
    /// Handles the enemy's death asynchronously.
    /// </summary>
    private async UniTask OnDeath()
    {
        // Trigger the orchestrated death logic
        _onDeathCallback?.Invoke();

        if (_isSpawner)
        {
            await SpawnOnDeathAsync();
        }
    }

    /// <summary>
    /// Spawns additional enemies on death asynchronously.
    /// </summary>
    private async UniTask SpawnOnDeathAsync()
    {
        if (_isSpawner && _spawnCount > 0)
        {
            for (int i = 0; i < _spawnCount; i++)
            {
                WaveConfig spawnConfig = new WaveConfig
                {
                    enemyType = _spawnType,
                    enemyCount = 1,
                    speed = _spawnSpeed,
                    health = _spawnHealth,
                    spawnInterval = 0f,
                    groupSpawn = false
                };

                await _enemySpawner.SpawnEnemyAsync(spawnConfig);
                await UniTask.Yield();
            }
        }
    }
}
