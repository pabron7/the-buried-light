using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class EnemyPoolManager : MonoBehaviour
{
    private readonly Dictionary<EnemyTypes, Queue<GameObject>> _enemyPools = new();
    private readonly Dictionary<EnemyTypes, GameObject> _enemyPrefabs = new();
    private DiContainer _container;

    [Inject]
    public void Construct(EnemyPrefabMapping[] mappings, DiContainer container)
    {
        _container = container ?? throw new System.ArgumentNullException(nameof(container));

        foreach (var mapping in mappings)
        {
            if (!_enemyPrefabs.ContainsKey(mapping.enemyType))
            {
                _enemyPrefabs[mapping.enemyType] = mapping.prefab;
                _enemyPools[mapping.enemyType] = new Queue<GameObject>();
            }
            else
            {
                Debug.LogWarning($"Duplicate mapping for {mapping.enemyType}. Skipping...");
            }
        }

        Debug.Log("EnemyPoolManager initialized with mappings.");
    }

    public GameObject GetEnemy(EnemyTypes type)
    {
        if (_enemyPools.TryGetValue(type, out var pool) && pool.Count > 0)
        {
            var enemy = pool.Dequeue();
            enemy.SetActive(true);
            Debug.Log($"Enemy of type {type} retrieved from pool.");
            return enemy;
        }

        Debug.LogWarning($"Pool for {type} is empty. Creating new enemy.");
        return CreateEnemy(type);
    }

    public void ReturnEnemy(EnemyTypes type, GameObject enemy)
    {
        if (!_enemyPools.ContainsKey(type))
        {
            Debug.LogWarning($"No pool exists for enemy type {type}. Destroying the enemy.");
            Destroy(enemy);
            return;
        }

        enemy.SetActive(false);
        _enemyPools[type].Enqueue(enemy);
        Debug.Log($"Enemy of type {type} returned to pool.");
    }

    private GameObject CreateEnemy(EnemyTypes type)
    {
        if (!_enemyPrefabs.TryGetValue(type, out var prefab))
        {
            Debug.LogError($"No prefab found for enemy type: {type}");
            return null;
        }

        var enemy = _container.InstantiatePrefab(prefab);
        enemy.GetComponent<EnemyBase>()?.Initialize(type, 0, 0, Vector3.zero); // Default initialization
        Debug.Log($"New enemy of type {type} created.");
        return enemy;
    }
}
