using System.Collections.Generic;
using UnityEngine;
using System;

public class WavePoolManager
{
    private readonly List<WaveManager> _waveManagerPool = new();
    private readonly List<WaveManager> _activeWaveManagers = new();
    private readonly WaveManager.Factory _waveManagerFactory;

    public WavePoolManager(WaveManager.Factory waveManagerFactory)
    {
        _waveManagerFactory = waveManagerFactory ?? throw new ArgumentNullException(nameof(waveManagerFactory));
    }

    /// <summary>
    /// Initializes the pool of WaveManagers with a specified size.
    /// </summary>
    public void InitializePool(int poolSize)
    {
        for (int i = 0; i < poolSize; i++)
        {
            var waveManager = _waveManagerFactory.Create();
            waveManager.gameObject.SetActive(false); // Start inactive
            _waveManagerPool.Add(waveManager);
        }

        Debug.Log($"WavePoolManager: Initialized pool with {poolSize} WaveManagers.");
    }

    /// <summary>
    /// Retrieves an available WaveManager from the pool.
    /// </summary>
    public WaveManager GetAvailableWaveManager()
    {
        if (_waveManagerPool.Count > 0)
        {
            var waveManager = _waveManagerPool[0];
            _waveManagerPool.RemoveAt(0);
            return waveManager;
        }

        // Dynamically expand the pool
        Debug.LogWarning("WavePoolManager: No available WaveManager in the pool, creating a new instance.");
        var newWaveManager = _waveManagerFactory.Create();
        newWaveManager.gameObject.SetActive(false);
        return newWaveManager;
    }


    /// <summary>
    /// Returns a WaveManager back to the pool and resets it.
    /// </summary>
    public void ReturnWaveManagerToPool(WaveManager waveManager)
    {
        if (waveManager == null || waveManager.gameObject == null)
        {
            Debug.LogError("WavePoolManager: Attempted to return a destroyed WaveManager to the pool.");
            return;
        }

        waveManager.ResetWave();
        waveManager.gameObject.SetActive(false);

        _activeWaveManagers.Remove(waveManager);
        _waveManagerPool.Add(waveManager);
    }


    /// <summary>
    /// Resets all active WaveManagers and returns them to the pool.
    /// </summary>
    public void ResetPool()
    {
        for (int i = _activeWaveManagers.Count - 1; i >= 0; i--)
        {
            ReturnWaveManagerToPool(_activeWaveManagers[i]);
        }

        _activeWaveManagers.Clear();
        Debug.Log("WavePoolManager: All WaveManagers have been reset and returned to the pool.");
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public WaveManager CreateAndAddWaveManagerToPool()
    {
        var newWaveManager = _waveManagerFactory.Create();
        newWaveManager.gameObject.SetActive(false);
        _waveManagerPool.Add(newWaveManager);
        return newWaveManager;
    }

    /// <summary>
    /// Retrieves the list of currently active WaveManagers.
    /// </summary>
    public List<WaveManager> GetActiveWaveManagers() => _activeWaveManagers;
}