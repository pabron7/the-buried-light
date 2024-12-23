using UnityEngine;
using Zenject;

public class Projectile : MonoBehaviour, IProjectile, IWrappable
{
    [SerializeField] private float speed = 10f;
    [SerializeField] private float lifeTime = 2f;
    [SerializeField] private int damage = 1;

    private ProjectilePoolManager _poolManager;
    private EnemyEvents _enemyEvents;
    private WrappingUtils _wrappingUtils;

    private float _spawnTime;

    [Inject]
    public void Construct(EnemyEvents enemyEvents, ProjectilePoolManager poolManager, WrappingUtils wrappingUtils)
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
        _poolManager.ReturnProjectile(this);
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
        transform.position = _wrappingUtils.WrapPosition(transform.position);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<IKillable>(out var killable))
        {
            _enemyEvents.NotifyEnemyDamaged(damage);
            ReturnToPool();
        }
    }

    public void OnHit()
    {
        
    }
}
