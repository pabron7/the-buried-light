using UnityEngine;
using Zenject;
using Cysharp.Threading.Tasks;
using System;

/// <summary>
/// Base class for enemies. Handles movement and orchestrates components.
/// </summary>
public abstract class EnemyBase : MonoBehaviour, IKillable, IScoreGiver
{
    public EnemyTypes Type { get; private set; }
    public int Health => _health.CurrentHealth;
    public float Speed { get; private set; }
    public int ScoreValue => scoreValue;
   

    [SerializeField] private int scoreValue = 10;
    [SerializeField] private int damage = 1;

    [Header("Visual Effect Settings")]
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Color flashColor = Color.white;
    [SerializeField] private float flashDuration = 0.1f;

    private Color _originalColor;

    // Dependencies
    protected GameFrame _gameFrame;
    private EnemyEvents _enemyEvents;
    private WrappingUtils _wrappingUtils;
    [Inject] private PlayerHealth _playerHealth;

    // Components
    private EnemyMovement _movement;
    private EnemyHealth _health;

    [Inject]
    public void Construct(GameFrame gameFrame, EnemySpawner enemySpawner, EnemyEvents enemyEvents, WrappingUtils wrappingUtils)
    {
        _gameFrame = gameFrame ?? throw new ArgumentNullException(nameof(gameFrame));
        _enemyEvents = enemyEvents ?? throw new ArgumentNullException(nameof(enemyEvents));
        _wrappingUtils = wrappingUtils ?? throw new ArgumentNullException(nameof(wrappingUtils));

        _health = GetComponent<EnemyHealth>();
        if (_health == null)
        {
            Debug.LogWarning("EnemyHealth component is missing. Attaching a new one dynamically.");
            _health = gameObject.AddComponent<EnemyHealth>();
        }

        _health.Initialize(0, enemySpawner, _enemyEvents, TriggerFlashEffect, OnDeath);
    }

    private void Awake()
    {
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

        _movement.Initialize(Speed, direction);
        _movement.SetDependencies(_wrappingUtils, _gameFrame);

        _health.Initialize(health, null, _enemyEvents, TriggerFlashEffect, OnDeath);
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
        _health.ConfigureOnDeathSpawn(spawnType, spawnCount, spawnHealth, spawnSpeed);
    }

    /// <summary>
    /// Applies damage to the enemy.
    /// </summary>
    public void TakeDamage(int damage)
    {
        _health.TakeDamage(damage);
    }

    /// <summary>
    /// Handles the enemy's death.
    /// </summary>
    public virtual void OnDeath()
    {
        _enemyEvents.NotifyEnemyKilled(transform.position);
        _enemyEvents.NotifyEnemyScore(this);
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
    /// Deactivates the enemy (for pooling).
    /// </summary>
    protected virtual void Deactivate()
    {
        gameObject.SetActive(false);
    }

    /// <summary>
    /// Handles collisions with other objects.
    /// </summary>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            OnDeath();
            _playerHealth.TakeDamage(damage);
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
