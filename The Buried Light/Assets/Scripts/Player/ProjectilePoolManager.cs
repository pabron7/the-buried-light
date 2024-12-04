using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class ProjectilePoolManager : MonoBehaviour
{
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private int poolSize = 20;

    private Queue<GameObject> projectilePool;

    [Inject]
    private DiContainer _container;

    private void Awake()
    {
        InitializePool();
    }

    private void InitializePool()
    {
        projectilePool = new Queue<GameObject>();

        for (int i = 0; i < poolSize; i++)
        {
            GameObject projectile = CreateProjectile();
            projectile.SetActive(false);
            projectilePool.Enqueue(projectile);
        }
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

        Debug.LogWarning("Projectile pool is empty. Consider increasing pool size.");
        GameObject newProjectile = CreateProjectile(); // Dynamically expand the pool if needed
        return newProjectile;
    }

    public void ReturnProjectile(GameObject projectile)
    {
        projectile.SetActive(false);
        projectilePool.Enqueue(projectile);
    }
}
