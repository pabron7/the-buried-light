using UnityEngine;
using UniRx;
using Zenject;
using System.Collections.Generic;

public class VFXManager : MonoBehaviour
{
    private VFXRegistry _vfxRegistry;
    private readonly Dictionary<string, Queue<GameObject>> _vfxPools = new();

    [Inject]
    public void Construct(VFXRegistry vfxRegistry, EnemyEvents enemyEvents, PlayerEvents playerEvents)
    {
        _vfxRegistry = vfxRegistry;

        // Subscribe to events
        enemyEvents.OnEnemyKilled
            .Subscribe(position => PlayVFX("enemy_killed", position))
            .AddTo(this);

        enemyEvents.OnEnemyDamaged
            .Subscribe(damage => PlayVFX("enemy_damage", Vector3.zero)) // Example: Use actual position
            .AddTo(this);

        playerEvents.OnPlayerShot
            .Subscribe(_ => PlayVFX("player_shot", Vector3.zero)) // Example: Use actual position
            .AddTo(this);
    }

    /// <summary>
    /// Plays a VFX at the specified position.
    /// </summary>
    public void PlayVFX(string vfxId, Vector3 position)
    {
        var vfxEffect = _vfxRegistry.GetVFXEffect(vfxId);
        if (vfxEffect == null)
        {
            Debug.LogWarning($"VFXManager: VFX with ID '{vfxId}' not found.");
            return;
        }

        var vfxObject = GetFromPool(vfxId, vfxEffect.Prefab);
        if (vfxObject == null)
        {
            Debug.LogWarning($"VFXManager: Failed to retrieve VFX for ID '{vfxId}'.");
            return;
        }

        vfxObject.transform.position = position;
        vfxObject.SetActive(true);

        // Automatically return to pool after a delay
        StartCoroutine(ReturnToPoolAfterDelay(vfxObject, vfxId, 0.4f)); // Example delay
    }

    /// <summary>
    /// Retrieves a VFX object from the pool or instantiates a new one.
    /// </summary>
    private GameObject GetFromPool(string vfxId, GameObject prefab)
    {
        if (!_vfxPools.TryGetValue(vfxId, out var pool))
        {
            pool = new Queue<GameObject>();
            _vfxPools[vfxId] = pool;
        }

        if (pool.Count > 0)
        {
            return pool.Dequeue();
        }

        // Instantiate a new VFX object if none are available in the pool
        var vfxObject = Instantiate(prefab);
        vfxObject.SetActive(false);
        return vfxObject;
    }

    /// <summary>
    /// Returns a VFX object to the pool after a delay.
    /// </summary>
    private System.Collections.IEnumerator ReturnToPoolAfterDelay(GameObject vfxObject, string vfxId, float delay)
    {
        yield return new WaitForSeconds(delay);

        if (vfxObject == null) yield break;

        vfxObject.SetActive(false);

        if (!_vfxPools.TryGetValue(vfxId, out var pool))
        {
            pool = new Queue<GameObject>();
            _vfxPools[vfxId] = pool;
        }

        pool.Enqueue(vfxObject);
    }
}
