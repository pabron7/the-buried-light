using System.Collections.Generic;
using UnityEngine;

public class EnemyFactory
{
    private readonly Dictionary<EnemyTypes, GameObject> _enemyPrefabMap;

    public EnemyFactory(EnemyPrefabMapping[] enemyPrefabs)
    {
        _enemyPrefabMap = new Dictionary<EnemyTypes, GameObject>();

        foreach (var mapping in enemyPrefabs)
        {
            if (!_enemyPrefabMap.ContainsKey(mapping.type))
            {
                _enemyPrefabMap.Add(mapping.type, mapping.prefab);
            }
            else
            {
                Debug.LogWarning($"Duplicate prefab for enemy type {mapping.type}. Skipping...");
            }
        }
    }

    public EnemyBase Create(EnemyTypes type)
    {
        if (_enemyPrefabMap.TryGetValue(type, out var prefab))
        {
            var instance = Object.Instantiate(prefab);
            return instance.GetComponent<EnemyBase>();
        }

        Debug.LogError($"Enemy type {type} does not exist in prefabs!");
        return null;
    }
}
