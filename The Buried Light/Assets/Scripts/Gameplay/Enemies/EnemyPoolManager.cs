using System.Collections.Generic;
using UnityEngine;
using Zenject;
using Cysharp.Threading.Tasks;

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
        }

        Debug.Log("EnemyPoolManager initialized with mappings.");
    }

    public async UniTask PreloadEnemiesAsync(EnemyTypes type, int count)
    {
        if (!_enemyPrefabs.ContainsKey(type)) return;

        for (int i = 0; i < count; i++)
        {
            var enemy = CreateEnemy(type);
            if (enemy != null)
            {
                ReturnEnemy(type, enemy);
                await UniTask.Yield(); // Yield to prevent blocking
            }
        }
    }

    public async UniTask<GameObject> GetEnemyAsync(EnemyTypes type)
    {
        if (_enemyPools.TryGetValue(type, out var pool) && pool.Count > 0)
        {
            var enemy = pool.Dequeue();
            enemy.SetActive(true);
            return enemy;
        }

        //Debug.LogWarning($"Pool for {type} is empty. Creating new enemy on the main thread.");
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
    }

    private GameObject CreateEnemy(EnemyTypes type)
    {
        if (!_enemyPrefabs.TryGetValue(type, out var prefab))
        {
            Debug.LogError($"No prefab found for enemy type: {type}");
            return null;
        }

        // makes sure instantiation happens on the main thread
        var enemy = _container.InstantiatePrefab(prefab);
        enemy.GetComponent<EnemyBase>()?.Initialize(type, 0, 0, Vector3.zero); // Default initialization
        //Debug.Log($"New enemy of type {type} created.");
        return enemy;
    }

}