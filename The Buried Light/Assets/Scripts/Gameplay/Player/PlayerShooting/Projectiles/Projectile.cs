using UnityEngine;
using Zenject;

public class Projectile : MonoBehaviour, IProjectile
{
    [SerializeField] private float speed = 10f;
    [SerializeField] private float lifeTime = 2f;
    [SerializeField] private int damage = 1;

    private ProjectilePoolManager _poolManager;
    private EnemyEvents _enemyEvents;

    [Inject]
    public void Construct(EnemyEvents enemyEvents)
    {
        _enemyEvents = enemyEvents;
    }

    public int Damage => damage;

    public void Initialize(ProjectilePoolManager poolManager)
    {
        _poolManager = poolManager;
    }

    private void Update()
    {
        Move();
    }

    private void Move()
    {
        transform.Translate(Vector3.up * speed * Time.deltaTime);
    }

    private void OnEnable()
    {
        Invoke(nameof(ReturnToPool), lifeTime);
    }

    private void OnDisable()
    {
        CancelInvoke();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<IKillable>(out var killable))
        {
            // Directly apply damage
            killable.TakeDamage(damage);

            // Notify the event system about the damage
            _enemyEvents.NotifyEnemyDamaged(damage);

            // Notify projectile of hit
            OnHit();
        }
        else
        {
            Debug.LogWarning($"Projectile collided with {collision.name}, but no IKillable component was found.");
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
}