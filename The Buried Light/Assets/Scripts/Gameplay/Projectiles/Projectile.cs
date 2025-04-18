using UnityEngine;
using Zenject;
using Cysharp.Threading.Tasks;
using System.Threading;
using System;

public class Projectile : MonoBehaviour, IProjectile, IWrappable
{
    [SerializeField] private ProjectileStats stats = new ProjectileStats(8f, 0.8f, 1, 3);

    private float _spawnTime;
    private int _currentHealth;
    private bool _isActive;
    private CancellationTokenSource _cancellationTokenSource;

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

    private async UniTaskVoid HandleLifeTime(CancellationToken token)
    {
        if (stats.LifeTime <= 0)
        {
            Debug.LogError("Projectile lifetime is invalid! Returning to pool.");
            ReturnToPool();
            return;
        }

        try
        {
            await UniTask.Delay((int)(stats.LifeTime * 1000), cancellationToken: token);
        }
        catch (OperationCanceledException)
        {
            return; // Exit safely if canceled
        }

        // Before returning to pool, check if this object is still valid
        if (!_isActive || this == null || gameObject == null || !gameObject.activeInHierarchy) return;
        ReturnToPool();
    }

    private void ReturnToPool()
    {
        if (!_isActive) return;
        _isActive = false;
        _cancellationTokenSource?.Cancel();
        _poolManager.Value.ReturnProjectile(this);
    }

    public void Reset()
    {
        _spawnTime = Time.time;
        _currentHealth = stats.Health;
        _isActive = false;
        gameObject.SetActive(false);
        _cancellationTokenSource?.Cancel();
    }

    public void Activate(Vector3 position, Quaternion rotation)
    {
        transform.position = position;
        transform.rotation = rotation;
        _spawnTime = Time.time;
        _currentHealth = stats.Health;
        _isActive = true;
        gameObject.SetActive(true);

        // Start async life tracking
        _cancellationTokenSource = new CancellationTokenSource();
        HandleLifeTime(_cancellationTokenSource.Token).Forget();
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
