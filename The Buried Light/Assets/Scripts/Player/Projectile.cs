using UnityEngine;

public class Projectile : MonoBehaviour, IProjectile
{
    [SerializeField] private float speed = 10f;
    [SerializeField] private float lifeTime = 2f;
    [SerializeField] private int damage = 1;

    private ProjectilePoolManager _poolManager;

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

    /// <summary>
    /// Fallback in case the pool manager is missing
    /// </summary>
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
