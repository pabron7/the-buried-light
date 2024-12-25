using UnityEngine;
using Zenject;
using Cysharp.Threading.Tasks;
using System;

public abstract class EnemyBase : MonoBehaviour, IKillable, IScoreGiver, IWrappable
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

    public int ScoreValue => scoreValue;

    // Dependencies
    protected GameFrame _gameFrame;
    protected EnemySpawner _enemySpawner;
    private EnemyEvents _enemyEvents;
    private WrappingUtils _wrappingUtils;

    [SerializeField] private int scoreValue = 10;

    [Header("Visual Effect Settings")]
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Color flashColor = Color.white;
    [SerializeField] private float flashDuration = 0.1f;

    private Color _originalColor;

    [Inject]
    public void Construct(GameFrame gameFrame, EnemySpawner enemySpawner, EnemyEvents enemyEvents, WrappingUtils wrappingUtils)
    {
        _gameFrame = gameFrame ?? throw new ArgumentNullException(nameof(gameFrame));
        _enemySpawner = enemySpawner ?? throw new ArgumentNullException(nameof(enemySpawner));
        _enemyEvents = enemyEvents ?? throw new ArgumentNullException(nameof(enemyEvents));
        _wrappingUtils = wrappingUtils ?? throw new ArgumentNullException(nameof(wrappingUtils));

        Debug.Log($"Dependencies injected for {gameObject.name}");
    }

    private void Awake()
    {
        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            if (spriteRenderer == null)
            {
                Debug.LogError("EnemyBase: SpriteRenderer not found. Please assign one.");
            }
        }
        _originalColor = spriteRenderer.color;
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
        WrapIfOutOfBounds();
    }

    /// <summary>
    /// Moves the enemy based on its speed and direction.
    /// </summary>
    protected virtual void Move()
    {
        transform.position += _direction * Speed * Time.deltaTime;
    }

    /// <summary>
    /// Applies damage to the enemy and triggers the flash effect.
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

        // Trigger flash effect
        TriggerFlashEffect();

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

        _enemyEvents.NotifyEnemyKilled(transform.position);         // Notify death event
        _enemyEvents.NotifyEnemyScore(this);    // Notify score event

        if (_isSpawner)
        {
            SpawnOnDeathAsync().Forget();
        }

        Deactivate();
    }

    /// <summary>
    /// Triggers the visual flash effect when the enemy is damaged.
    /// </summary>
    private async void TriggerFlashEffect()
    {
        if (spriteRenderer == null) return;

        spriteRenderer.color = flashColor;
        await UniTask.Delay((int)(flashDuration * 1000));
        spriteRenderer.color = _originalColor;
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
    /// Wraps the enemy around the screen if it goes out of bounds.
    /// </summary>
    public void WrapIfOutOfBounds()
    {
        if (_gameFrame != null)
        {
            transform.position = _wrappingUtils.WrapPosition(transform.position);
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
