using UnityEngine;

public abstract class EnemyBase : MonoBehaviour
{
    public int Health { get; private set; }
    public float Speed { get; private set; }
    public int Damage { get; private set; }
    public EnemyTypes Type { get; private set; }

    private Vector3 _direction;

    // On-death spawn parameters
    protected bool _isSpawner { get; private set; }
    public EnemyTypes _spawnType { get; private set; }
    public int _spawnCount { get; private set; }
    public int _spawnHealth { get; private set; }
    public float _spawnSpeed { get; private set; }

    /// <summary>
    /// Initialize the enemy with core attributes and movement direction.
    /// </summary>
    public virtual void Initialize(
        float speed,
        Vector3 direction,
        int health,
        int damage,
        bool isSpawner,
        EnemyTypes spawnType,
        int spawnCount,
        int spawnHealth,
        float spawnSpeed)
    {
        Speed = speed;
        _direction = direction.normalized; // Ensure direction is normalized
        Health = health;
        Damage = damage;

        // Configure on-death spawning
        _isSpawner = isSpawner;
        _spawnType = spawnType;
        _spawnCount = spawnCount;
        _spawnHealth = spawnHealth;
        _spawnSpeed = spawnSpeed;
    }

    protected void Update()
    {
        Move();
    }

    protected virtual void Move()
    {
        transform.position += _direction * Speed * Time.deltaTime;
    }

    public void TakeDamage(int amount)
    {
        Health -= amount;

        if (Health <= 0)
        {
            OnDeath();
        }
    }

    private void OnBecameInvisible()
    {
        Deactivate();
    }

    protected virtual void OnDeath()
    {
  //

        Deactivate();
    }

    protected void Deactivate()
    {
        gameObject.SetActive(false);
    }

    public abstract void OnContact();
}
