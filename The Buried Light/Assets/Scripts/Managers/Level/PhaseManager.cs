using System.Collections;
using UnityEngine;
using Zenject;

public class PhaseManager
{
    private readonly LevelConfig _levelConfig;
    private readonly WavePoolManager _wavePoolManager;
    private int _currentPhaseIndex = 0;
    private bool _isHalted = false;

    [Inject] private GameEvents _gameEvents;

    public PhaseManager(LevelConfig levelConfig, WavePoolManager wavePoolManager)
    {
        _levelConfig = levelConfig;
        _wavePoolManager = wavePoolManager;
    }

    /// <summary>
    /// The current phase index.
    /// </summary>
    public int CurrentPhaseIndex => _currentPhaseIndex;

    /// <summary>
    /// Determines if there are more phases to process in the level.
    /// </summary>
    /// <returns>True if more phases are available; otherwise, false.</returns>
    public bool HasMorePhases()
    {
        return _currentPhaseIndex < _levelConfig.phases.Length;
    }

    /// <summary>
    /// Starts the next phase by handling its waves sequentially.
    /// </summary>
    public IEnumerator StartPhase()
    {
        if (!HasMorePhases())
        {
            Debug.LogWarning("PhaseManager: No more phases available.");
            yield break;
        }

        var currentPhase = _levelConfig.phases[_currentPhaseIndex];
        _gameEvents.NotifyPhaseStart(_currentPhaseIndex); // Notify phase start
        _currentPhaseIndex++;

        Debug.Log($"PhaseManager: Starting Phase {_currentPhaseIndex}");
        yield return HandlePhase(currentPhase);

        _gameEvents.NotifyPhaseEnd(_currentPhaseIndex - 1); // Notify phase end
    }

    /// <summary>
    /// Handles the wave spawning logic for the current phase.
    /// </summary>
    private IEnumerator HandlePhase(PhaseConfig phaseConfig)
    {
        var activeWaveManagers = _wavePoolManager.GetActiveWaveManagers();
        activeWaveManagers.Clear();

        foreach (var waveConfig in phaseConfig.waves)
        {
            var waveManager = _wavePoolManager.GetAvailableWaveManager();
            if (waveManager == null)
            {
                Debug.LogError("PhaseManager: Failed to retrieve WaveManager.");
                yield break;
            }

            waveManager.Initialize(waveConfig);
            waveManager.OnWaveComplete += CheckWaveCompletion;
            waveManager.gameObject.SetActive(true);
            waveManager.StartWave();
            activeWaveManagers.Add(waveManager);
        }

        // Wait until all waves in the phase are complete
        while (activeWaveManagers.Exists(wm => wm.CurrentState != WaveManager.WaveState.WaveComplete))
        {
            yield return null;
        }

        Debug.Log($"PhaseManager: Phase {_currentPhaseIndex} complete.");
    }

    /// <summary>
    /// Checks if all waves in the current phase are complete.
    /// </summary>
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
            Debug.Log("PhaseManager: All waves in phase are complete.");
        }
    }

    /// <summary>
    /// Resets the phase progression, preparing for a new cycle.
    /// </summary>
    public void ResetPhases()
    {
        _currentPhaseIndex = 0;
        Debug.Log("PhaseManager: Phases reset.");
    }

    /// <summary>
    /// Halts the current phase and all active waves.
    /// </summary>
    public void HaltPhase()
    {
        Debug.Log("PhaseManager: Halting phase.");
        _isHalted = true;

        foreach (var waveManager in _wavePoolManager.GetActiveWaveManagers())
        {
            waveManager.HaltWave();
        }
    }

    /// <summary>
    /// Resumes the current phase and all active waves if halted.
    /// </summary>
    public void ContinuePhase()
    {
        Debug.Log("PhaseManager: Continuing phase.");
        _isHalted = false;

        foreach (var waveManager in _wavePoolManager.GetActiveWaveManagers())
        {
            waveManager.ContinueWave();
        }
    }

    /// <summary>
    /// Cancels the current phase and stops all active waves.
    /// </summary>
    public void CancelPhase()
    {
        Debug.Log("PhaseManager: Cancelling phase.");
        _isHalted = false;

        foreach (var waveManager in _wavePoolManager.GetActiveWaveManagers())
        {
            waveManager.CancelWave();
        }
    }
}
