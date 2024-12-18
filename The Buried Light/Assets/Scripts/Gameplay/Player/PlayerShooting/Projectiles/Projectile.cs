using UnityEngine;
using Zenject;

public class Projectile : MonoBehaviour, IProjectile
{
    [SerializeField] private float speed = 10f;
    [SerializeField] private float lifeTime = 2f;
    [SerializeField] private int damage = 1;

    private ProjectilePoolManager _poolManager;
    private EnemyEvents _enemyEvents;

    private float _spawnTime;

    [Inject]
    public void Construct(EnemyEvents enemyEvents, ProjectilePoolManager poolManager)
    {
        _enemyEvents = enemyEvents;
        _poolManager = poolManager; // Injected directly
    }

    public int Damage => damage;

    private void Update()
    {
        Move();
        CheckLifeTime();
    }

    private void Move()
    {
        transform.Translate(Vector3.up * speed * Time.deltaTime);
    }

    private void OnEnable()
    {
        _spawnTime = Time.time;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<IKillable>(out var killable))
        {
            // Apply damage
            killable.TakeDamage(damage);

            // Notify event system about damage
            _enemyEvents.NotifyEnemyDamaged(damage);

            // Handle projectile hit
            OnHit();
        }
        else
        {
            Debug.LogWarning($"Projectile collided with {collision.name}, but no IKillable component was found.");
        }
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
        if (_poolManager != null)
        {
            _poolManager.ReturnProjectile(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void OnHit()
    {
        Debug.Log("Projectile hit its target.");
        ReturnToPool();
    }

    public void OnDisable()
    {
        _spawnTime = Time.time;
    }
}
