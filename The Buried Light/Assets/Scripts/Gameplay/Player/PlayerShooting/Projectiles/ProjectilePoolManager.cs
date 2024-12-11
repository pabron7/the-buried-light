using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class ProjectilePoolManager : MonoBehaviour
{
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private int initialPoolSize = 5;

    private Queue<GameObject> projectilePool;
    private int currentPoolSize;

    [Inject]
    private DiContainer _container;

    private void Awake()
    {
        InitializePool();
    }

    private void InitializePool()
    {
        projectilePool = new Queue<GameObject>();
        currentPoolSize = initialPoolSize;

        for (int i = 0; i < initialPoolSize; i++)
        {
            AddProjectileToPool();
        }
    }

    private void AddProjectileToPool()
    {
        GameObject projectile = CreateProjectile();
        projectile.SetActive(false);
        projectilePool.Enqueue(projectile);
        currentPoolSize++;
    }

    private GameObject CreateProjectile()
    {
        GameObject projectile = _container.InstantiatePrefab(projectilePrefab);
        projectile.GetComponent<Projectile>().Initialize(this);
        return projectile;
    }

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

    public void ReturnProjectile(GameObject projectile)
    {
        projectile.SetActive(false);
        projectilePool.Enqueue(projectile);
    }
}
