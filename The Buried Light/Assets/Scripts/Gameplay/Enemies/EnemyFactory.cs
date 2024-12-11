using System.Collections.Generic;
using UnityEngine;
using Zenject;
using Cysharp.Threading.Tasks;

public class EnemyFactory
{
    private readonly Dictionary<EnemyTypes, GameObject> _enemyPrefabMap;
    private readonly DiContainer _container;

    [Inject]
    public EnemyFactory(EnemyPrefabMapping[] mappings, DiContainer container)
    {
        _container = container ?? throw new System.ArgumentNullException(nameof(container));
        _enemyPrefabMap = new Dictionary<EnemyTypes, GameObject>();

        foreach (var mapping in mappings)
        {
            if (!_enemyPrefabMap.ContainsKey(mapping.enemyType))
            {
                _enemyPrefabMap[mapping.enemyType] = mapping.prefab;
            }
        }
    }

    public async UniTask<GameObject> CreateAsync(EnemyTypes type)
    {
        if (_enemyPrefabMap.TryGetValue(type, out var prefab))
        {
            return await UniTask.Run(() => _container.InstantiatePrefab(prefab));
        }

        Debug.LogError($"No prefab mapped for enemy type: {type}");
        return null;
    }
}
