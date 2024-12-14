using System.Collections.Generic;
using UnityEngine;

public class WavePoolManager
{
    private readonly List<WaveManager> _waveManagerPool = new();
    private readonly List<WaveManager> _activeWaveManagers = new();
    private readonly WaveManager.Factory _waveManagerFactory;

    public WavePoolManager(WaveManager.Factory waveManagerFactory)
    {
        _waveManagerFactory = waveManagerFactory;
    }

    public void InitializePool(int poolSize)
    {
        for (int i = 0; i < poolSize; i++)
        {
            var waveManager = _waveManagerFactory.Create();
            waveManager.gameObject.SetActive(false); // Initially deactivate
            _waveManagerPool.Add(waveManager);
        }

        Debug.Log($"Wave manager pool initialized with {poolSize} managers.");
    }

    public WaveManager GetAvailableWaveManager()
    {
        if (_waveManagerPool.Count > 0)
        {
            var waveManager = _waveManagerPool[0];
            _waveManagerPool.RemoveAt(0);
            return waveManager;
        }

        Debug.LogError("No available WaveManager in the pool.");
        return null;
    }

    public void ReturnWaveManagerToPool(WaveManager waveManager)
    {
        waveManager.ResetWave();
        waveManager.gameObject.SetActive(false);
        _waveManagerPool.Add(waveManager);
    }

    public void ResetPool()
    {
        foreach (var waveManager in _activeWaveManagers)
        {
            waveManager.ResetWave();
            waveManager.gameObject.SetActive(false);
            _waveManagerPool.Add(waveManager);
        }

        _activeWaveManagers.Clear();
        Debug.Log("Wave manager pool has been reset.");
    }

    public List<WaveManager> GetActiveWaveManagers() => _activeWaveManagers;
}
