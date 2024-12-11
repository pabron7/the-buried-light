using UnityEngine;
using Zenject;
using Cysharp.Threading.Tasks;
using System;

public abstract class EnemyBase : MonoBehaviour, IKillable
{
    public EnemyTypes Type { get; private set; }
    public int Health { get; private set; }
    public float Speed { get; private set; }
    private Vector3 _direction;

    private bool _isSpawner;
    private EnemyTypes _spawnType;
    private int _spawnCount;
    private int _spawnHealth;
    private float _spawnSpeed;

    // Dependencies
    protected GameFrame _gameFrame;
    protected EnemySpawner _enemySpawner;
    private EnemyEvents _enemyEvents;

    [Inject]
    public void Construct(GameFrame gameFrame, EnemySpawner enemySpawner, EnemyEvents enemyEvents)
    {
        _gameFrame = gameFrame ?? throw new ArgumentNullException(nameof(gameFrame));
        _enemySpawner = enemySpawner ?? throw new ArgumentNullException(nameof(enemySpawner));
        _enemyEvents = enemyEvents ?? throw new ArgumentNullException(nameof(enemyEvents));

        Debug.Log($"Dependencies injected for {gameObject.name}");
    }

    /// <summary>
    /// Initialize the enemy with base stats and type.
    /// </summary>
    public void Initialize(EnemyTypes type, float speed, int health, Vector3 direction)
    {
        Type = type;
        Speed = speed;
        Health = health;
        _direction = direction.normalized;

        Debug.Log($"Initialized enemy of type {Type} with speed: {Speed}, health: {Health}");
    }

    /// <summary>
    /// Configure on-death spawning behavior.
    /// </summary>
    public void ConfigureOnDeathSpawn(EnemyTypes spawnType, int spawnCount, int spawnHealth, float spawnSpeed)
    {
        _isSpawner = true;
        _spawnType = spawnType;
        _spawnCount = spawnCount;
        _spawnHealth = spawnHealth;
        _spawnSpeed = spawnSpeed;

        Debug.Log($"Enemy {Type} configured to spawn {_spawnCount} {_spawnType} on death.");
    }

    private void Update()
    {
        Move();
        CheckOutOfBounds();
    }

    /// <summary>
    /// Moves the enemy based on its speed and direction.
    /// </summary>
    protected virtual void Move()
    {
        transform.position += _direction * Speed * Time.deltaTime;
    }

    /// <summary>
    /// Applies damage to the enemy.
    /// </summary>
    public void TakeDamage(int damage)
    {
        if (damage <= 0)
        {
            Debug.LogWarning($"Enemy {Type} received invalid damage value: {damage}");
            return;
        }

        Health -= damage;
        Debug.Log($"Enemy {Type} took {damage} damage. Remaining health: {Health}");

        // Notify damage event
        _enemyEvents.NotifyEnemyDamaged(damage);

        if (Health <= 0)
        {
            OnDeath();
        }
    }

    /// <summary>
    /// Handles the enemy's death.
    /// </summary>
    public virtual void OnDeath()
    {
        Debug.Log($"Enemy {Type} has died.");

        // Notify death event
        _enemyEvents.NotifyEnemyKilled(transform.position);

        if (_isSpawner)
        {
            SpawnOnDeathAsync().Forget();
        }

        Deactivate();
    }

    /// <summary>
    /// Spawns additional enemies on death asynchronously.
    /// </summary>
    protected virtual async UniTaskVoid SpawnOnDeathAsync()
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
                await UniTask.Yield(); // Yield to avoid blocking the main thread
            }
        }
    }

    /// <summary>
    /// Turns enemy off when gone out of game frame.
    /// </summary>
    private void CheckOutOfBounds()
    {
        if (transform.position.x < _gameFrame.DeletionMinBounds.x || transform.position.x > _gameFrame.DeletionMaxBounds.x ||
            transform.position.y < _gameFrame.DeletionMinBounds.y || transform.position.y > _gameFrame.DeletionMaxBounds.y)
        {
            Debug.Log($"Enemy {Type} went out of deletion boundaries and is being deactivated.");
            Deactivate();
        }
    }

    /// <summary>
    /// Deactivates the enemy (for pooling).
    /// </summary>
    protected virtual void Deactivate()
    {
        _enemySpawner.ReturnEnemy(Type, gameObject);
        Debug.Log($"Enemy {Type} returned to pool.");
    }

    /// <summary>
    /// Handles collisions with other objects.
    /// </summary>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            _enemyEvents.NotifyEnemyKilled(transform.position);
            OnDeath();
        }
        else if (collision.CompareTag("Projectile"))
        {
            if (collision.TryGetComponent<IProjectile>(out var projectile))
            {
                TakeDamage(projectile.Damage);
                projectile.OnHit();
            }
        }
    }
}
