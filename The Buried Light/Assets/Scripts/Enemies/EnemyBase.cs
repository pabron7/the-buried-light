using UnityEngine;

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
        Health -= damage;
        Debug.Log($"Enemy {Type} took {damage} damage. Remaining health: {Health}");

        if (Health <= 0)
        {
            OnDeath();
        }
    }

    /// <summary>
    /// Called when the enemy contacts another object.
    /// </summary>
    public virtual void OnContact(int damage)
    {
        TakeDamage(damage);
    }

    /// <summary>
    /// Handles the enemy's death.
    /// </summary>
    public virtual void OnDeath()
    {
        Debug.Log($"Enemy {Type} has died.");
        if (_isSpawner)
        {
            SpawnOnDeath();
        }

        Deactivate();
    }

    /// <summary>
    /// Spawns additional enemies on death.
    /// </summary>
    protected virtual void SpawnOnDeath()
    {
        Debug.Log($"Enemy {Type} spawning {_spawnCount} {_spawnType}.");
        // Custom logic for spawner enemies should go here
    }

    /// <summary>
    /// Deactivates the enemy (for pooling).
    /// </summary>
    protected virtual void Deactivate()
    {
        gameObject.SetActive(false);
        Debug.Log($"Enemy {Type} deactivated.");
    }
}