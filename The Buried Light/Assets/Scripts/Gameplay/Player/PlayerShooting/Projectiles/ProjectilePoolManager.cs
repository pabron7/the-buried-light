using System.Collections.Generic;
using UnityEngine;
using Zenject;

using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class ProjectilePoolManager : MonoBehaviour
{
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private int initialPoolSize = 5;

    private Queue<GameObject> projectilePool;

    [Inject]
    private DiContainer _container;

    private void Awake()
    {
        InitializePool();
    }

    /// <summary>
    /// Initializes the projectile pool with a predefined size.
    /// </summary>
    private void InitializePool()
    {
        projectilePool = new Queue<GameObject>();

        for (int i = 0; i < initialPoolSize; i++)
        {
            AddProjectileToPool();
        }
    }

    /// <summary>
    /// Adds a new projectile instance to the pool.
    /// </summary>
    private void AddProjectileToPool()
    {
        GameObject projectile = CreateProjectile();
        projectile.SetActive(false);
        projectilePool.Enqueue(projectile);
    }

    /// <summary>
    /// Creates a new projectile instance using Zenject.
    /// </summary>
    private GameObject CreateProjectile()
    {
        GameObject projectile = _container.InstantiatePrefab(projectilePrefab);
        return projectile;
    }

    /// <summary>
    /// Retrieves a projectile from the pool or expands the pool if empty.
    /// </summary>
    public GameObject GetProjectile()
    {
        if (projectilePool.Count > 0)
        {
            GameObject projectile = projectilePool.Dequeue();
            projectile.SetActive(true);
            return projectile;
        }

        Debug.LogWarning("Projectile pool is empty. Dynamically increasing pool size.");
        AddProjectileToPool();
        return GetProjectile();
    }

    /// <summary>
    /// Returns a projectile to the pool after resetting its state.
    /// </summary>
    public void ReturnProjectile(GameObject projectile)
    {
        if (projectile.TryGetComponent<Projectile>(out var proj))
        {
            proj.OnDisable(); // Ensures proper reset of projectile state
        }

        projectile.SetActive(false);
        projectilePool.Enqueue(projectile);
    }
}
