using UnityEngine;
using Zenject;
using Cysharp.Threading.Tasks;
using System;

/// <summary>
/// Base class for enemies. Handles core functionality such as health, damage, movement, and death.
/// </summary>
public abstract class EnemyBase : MonoBehaviour, IKillable, IScoreGiver
{
    public EnemyTypes Type { get; private set; }
    public int Health { get; private set; }
    public float Speed { get; private set; }
    public int ScoreValue => scoreValue;

    private bool _isSpawner;
    private EnemyTypes _spawnType;
    private int _spawnCount;
    private int _spawnHealth;
    private float _spawnSpeed;

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

    // Movement Component
    private EnemyMovement _movement;

    [Inject]
    public void Construct(GameFrame gameFrame, EnemySpawner enemySpawner, EnemyEvents enemyEvents, WrappingUtils wrappingUtils)
    {
        _gameFrame = gameFrame ?? throw new ArgumentNullException(nameof(gameFrame));
        _enemySpawner = enemySpawner ?? throw new ArgumentNullException(nameof(enemySpawner));
        _enemyEvents = enemyEvents ?? throw new ArgumentNullException(nameof(enemyEvents));
        _wrappingUtils = wrappingUtils ?? throw new ArgumentNullException(nameof(wrappingUtils));
    }

    private void Awake()
    {
        // Ensure the EnemyMovement component is attached
        _movement = GetComponent<EnemyMovement>();
        if (_movement == null)
        {
            Debug.LogWarning("EnemyMovement component is missing. Attaching a new one dynamically.");
            _movement = gameObject.AddComponent<EnemyMovement>();
        }

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
    /// Initializes the enemy with base stats and type.
    /// </summary>
    public void Initialize(EnemyTypes type, float speed, int health, Vector3 direction)
    {
        Type = type;
        Speed = speed;
        Health = health;

        if (_movement == null)
        {
            Debug.LogError("EnemyMovement component is missing or not initialized.");
            return;
        }

        _movement.Initialize(Speed, direction);
        _movement.SetDependencies(_wrappingUtils, _gameFrame);
    }


    private void Update()
    {
        _movement.Move();
        _movement.WrapIfOutOfBounds();
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
    /// Applies damage to the enemy and triggers the flash effect.
    /// </summary>
    public void TakeDamage(int damage)
    {
        if (damage <= 0)
        {
            return;
        }

        Health -= damage;

        TriggerFlashEffect();

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
        _enemyEvents.NotifyEnemyKilled(transform.position);
        _enemyEvents.NotifyEnemyScore(this);

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
                await UniTask.Yield();
            }
        }
    }

    /// <summary>
    /// Deactivates the enemy (for pooling).
    /// </summary>
    protected virtual void Deactivate()
    {
        _enemySpawner.ReturnEnemy(Type, gameObject);
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
