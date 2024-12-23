using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class ProjectilePoolManager : MonoBehaviour
{
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private int initialPoolSize = 10;

    private Queue<Projectile> _projectilePool;
    private DiContainer _container;

    [Inject]
    public void Construct(DiContainer container)
    {
        _container = container;
        InitializePool();
    }

    private void InitializePool()
    {
        _projectilePool = new Queue<Projectile>();

        for (int i = 0; i < initialPoolSize; i++)
        {
            AddProjectileToPool();
        }
    }

    private void AddProjectileToPool()
    {
        GameObject projectileObject = _container.InstantiatePrefab(projectilePrefab);
        Projectile projectile = projectileObject.GetComponent<Projectile>();

        if (projectile == null)
        {
            Debug.LogError("Projectile prefab is missing the Projectile component!");
            return;
        }

        projectile.Reset();
        _projectilePool.Enqueue(projectile);
    }

    public Projectile GetProjectile()
    {
        if (_projectilePool.Count > 0)
        {
            return _projectilePool.Dequeue();
        }

        Debug.LogWarning("Expanding the projectile pool.");
        AddProjectileToPool();
        return GetProjectile();
    }

    public void ReturnProjectile(Projectile projectile)
    {
        projectile.Reset();
        _projectilePool.Enqueue(projectile);
    }
}
