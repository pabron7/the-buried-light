using UnityEngine;
using Zenject;

public class Projectile : MonoBehaviour, IProjectile, IWrappable
{
    [SerializeField] private ProjectileStats stats = new ProjectileStats(8f, 0.8f, 1, 3); 

    private float _spawnTime;
    private int _currentHealth;
    private bool _isActive;

    private LazyInject<ProjectilePoolManager> _poolManager;
    private LazyInject<EnemyEvents> _enemyEvents;
    private LazyInject<WrappingUtils> _wrappingUtils;

    [Inject]
    public void Construct(
        LazyInject<EnemyEvents> enemyEvents,
        LazyInject<ProjectilePoolManager> poolManager,
        LazyInject<WrappingUtils> wrappingUtils)
    {
        _enemyEvents = enemyEvents;
        _poolManager = poolManager;
        _wrappingUtils = wrappingUtils;
    }

    public int Damage => stats.Damage;

    private void Update()
    {
        if (!_isActive) return;

        CheckLifeTime();

        if (_currentHealth > 0)
        {
            Move();
            WrapIfOutOfBounds();
        }
    }

    private void Move()
    {
        transform.Translate(Vector3.up * stats.Speed * Time.deltaTime);
    }

    private void CheckLifeTime()
    {
        if (stats.LifeTime <= 0)
        {
            Debug.LogError("Projectile lifetime is invalid! Returning to pool.");
            ReturnToPool();
            return;
        }

        if (Time.time >= _spawnTime + stats.LifeTime)
        {
            ReturnToPool();
        }
    }

    private void ReturnToPool()
    {
        if (!_isActive) return;
        _isActive = false;
        _poolManager.Value.ReturnProjectile(this);
    }

    public void Reset()
    {
        _spawnTime = Time.time;
        _currentHealth = stats.Health;
        _isActive = false;
        gameObject.SetActive(false);
    }

    public void Activate(Vector3 position, Quaternion rotation)
    {
        transform.position = position;
        transform.rotation = rotation;
        _spawnTime = Time.time;
        _currentHealth = stats.Health;
        _isActive = true;
        gameObject.SetActive(true);
    }

    public void WrapIfOutOfBounds()
    {
        transform.position = _wrappingUtils.Value.WrapPosition(transform.position);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<IKillable>(out var killable))
        {
            _enemyEvents.Value.NotifyEnemyDamaged(stats.Damage);
            OnHit();
        }
    }

    public void OnHit()
    {
        _currentHealth--;

        if (_currentHealth <= 0)
        {
            ReturnToPool();
        }
        else
        {
            Debug.Log($"Projectile hit an enemy. Remaining health: {_currentHealth}");
        }
    }
}
