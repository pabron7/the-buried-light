using UnityEngine;
using UniRx;
using Zenject;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using System.Threading;
using System;

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

        // Use a cancellation token in case the VFX object is destroyed before delay ends
        var token = this.GetCancellationTokenOnDestroy();
        ReturnToPoolAfterDelay(vfxObject, vfxId, 0.4f, token).Forget();
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
    private async UniTaskVoid ReturnToPoolAfterDelay(GameObject vfxObject, string vfxId, float delay, CancellationToken token)
    {
        try
        {
            await UniTask.Delay((int)(delay * 1000), cancellationToken: token);

            if (vfxObject == null || !vfxObject.activeInHierarchy) return;

            vfxObject.SetActive(false);

            if (!_vfxPools.TryGetValue(vfxId, out var pool))
            {
                pool = new Queue<GameObject>();
                _vfxPools[vfxId] = pool;
            }

            pool.Enqueue(vfxObject);
        }
        catch (OperationCanceledException)
        {
            return; // Exit safely if canceled
        }
    }
}
