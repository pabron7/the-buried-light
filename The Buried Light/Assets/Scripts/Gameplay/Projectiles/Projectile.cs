using UnityEngine;
using Zenject;

public class Projectile : MonoBehaviour, IProjectile, IWrappable
{
    [SerializeField] private float speed = 10f;
    [SerializeField] private float lifeTime = 2f;
    [SerializeField] private int damage = 1;

    private float _spawnTime;

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

    public int Damage => damage;

    private void Update()
    {
        Move();
        CheckLifeTime();
        WrapIfOutOfBounds();
    }

    private void Move()
    {
        transform.Translate(Vector3.up * speed * Time.deltaTime);
    }

    private void CheckLifeTime()
    {
        if (Time.time >= _spawnTime + lifeTime)
        {
            ReturnToPool();
        }
    }

    private void ReturnToPool()
    {
        _poolManager.Value.ReturnProjectile(this);
    }

    public void Reset()
    {
        _spawnTime = Time.time;
        gameObject.SetActive(false);
    }

    public void Activate(Vector3 position, Quaternion rotation)
    {
        transform.position = position;
        transform.rotation = rotation;
        _spawnTime = Time.time;
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
            _enemyEvents.Value.NotifyEnemyDamaged(damage);
            ReturnToPool();
        }
    }

    public void OnHit()
    {
    }
}
