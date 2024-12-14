using System.Collections;
using UnityEngine;

public class PhaseManager
{
    private readonly LevelConfig _levelConfig;
    private readonly WavePoolManager _wavePoolManager;
    private int _currentPhaseIndex = 0;

    public PhaseManager(LevelConfig levelConfig, WavePoolManager wavePoolManager)
    {
        _levelConfig = levelConfig;
        _wavePoolManager = wavePoolManager;
    }

    public bool HasMorePhases()
    {
        return _currentPhaseIndex < _levelConfig.phases.Length;
    }

    public IEnumerator StartPhase()
    {
        if (!HasMorePhases())
        {
            Debug.LogWarning("No more phases available.");
            yield break;
        }

        var currentPhase = _levelConfig.phases[_currentPhaseIndex];
        _currentPhaseIndex++;

        Debug.Log($"Starting Phase {_currentPhaseIndex}");
        yield return HandlePhase(currentPhase);
    }

    private IEnumerator HandlePhase(PhaseConfig phaseConfig)
    {
        var activeWaveManagers = _wavePoolManager.GetActiveWaveManagers();
        activeWaveManagers.Clear();

        foreach (var waveConfig in phaseConfig.waves)
        {
            var waveManager = _wavePoolManager.GetAvailableWaveManager();
            if (waveManager == null)
            {
                Debug.LogError("Failed to retrieve WaveManager.");
                yield break;
            }

            waveManager.Initialize(waveConfig);
            waveManager.OnWaveComplete += CheckWaveCompletion;
            waveManager.gameObject.SetActive(true);
            waveManager.StartWave();
            activeWaveManagers.Add(waveManager);
        }

        // Wait until all waves are complete
        while (activeWaveManagers.Exists(wm => wm.CurrentState != WaveManager.WaveState.WaveComplete))
        {
            yield return null;
        }

        Debug.Log($"Phase {_currentPhaseIndex} complete.");
    }

    private void CheckWaveCompletion()
    {
        var activeWaveManagers = _wavePoolManager.GetActiveWaveManagers();

        if (activeWaveManagers.TrueForAll(wm => wm.CurrentState == WaveManager.WaveState.WaveComplete))
        {
            foreach (var waveManager in activeWaveManagers)
            {
                _wavePoolManager.ReturnWaveManagerToPool(waveManager);
            }

            activeWaveManagers.Clear();
            Debug.Log("All waves in phase are complete.");
        }
    }

    public void ResetPhases()
    {
        _currentPhaseIndex = 0;
        Debug.Log("PhaseManager: Reset phases.");
    }
}
