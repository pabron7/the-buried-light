using UnityEngine;
using Zenject;
using Cysharp.Threading.Tasks;
using System.Collections.Generic;

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

    /// <summary> The current wave index. </summary>
    public int CurrentPhaseIndex => _currentPhaseIndex;

    /// <summary> The current phase index. </summary>
    public int TotalPhases => _levelConfig.phases.Length;

    /// <summary>
    /// Determines if there are more phases to process in the level.
    /// </summary>
    public bool HasMorePhases() { return _currentPhaseIndex < TotalPhases; }

    /// <summary>
    /// Starts the next phase asynchronously and manages wave spawning.
    /// </summary>
    public async UniTask StartPhaseAsync()
    {
        if (!HasMorePhases())
        {
            Debug.LogWarning("PhaseManager: No more phases available.");
            return;
        }

        var currentPhase = _levelConfig.phases[_currentPhaseIndex];
        _currentPhaseIndex++;

        Debug.Log($"PhaseManager: Starting Phase {_currentPhaseIndex}");
        _gameEvents.NotifyPhaseStart(_currentPhaseIndex);

        // Activate all waves in the phase simultaneously
        var activeWaveManagers = new List<WaveManager>();
        foreach (var waveConfig in currentPhase.waves)
        {
            var waveManager = _wavePoolManager.GetAvailableWaveManager();
            if (waveManager == null)
            {
                Debug.LogError("PhaseManager: Failed to retrieve WaveManager.");
                continue;
            }

            waveManager.Initialize(waveConfig);
            waveManager.gameObject.SetActive(true);
            waveManager.StartWave();
            activeWaveManagers.Add(waveManager);
        }

        // Wait until all waves are complete
        await UniTask.WaitUntil(() =>
            activeWaveManagers.TrueForAll(wm => wm.CurrentState == WaveManager.WaveState.WaveComplete)
        );

        // Cleanup and notify phase completion
        foreach (var waveManager in activeWaveManagers)
        {
            waveManager.ResetWave();
            waveManager.gameObject.SetActive(false);
            _wavePoolManager.ReturnWaveManagerToPool(waveManager);
        }

        Debug.Log("PhaseManager: Phase complete.");
        _gameEvents.NotifyPhaseEnd(_currentPhaseIndex);
    }

    /// <summary>
    /// Resets the phase progression for a new cycle.
    /// </summary>
    public void ResetPhases()
    {
        _currentPhaseIndex = 0;
        LogDebug("Phases reset.");
    }

    /// <summary>
    /// 
    /// </summary>
    public void ResetWaves()
    {
        LogDebug("Resetting all waves via PhaseManager.");
        _wavePoolManager.ResetPool();
    }

    public void HaltPhase()
    {
        _isHalted = true;
        foreach (var waveManager in _wavePoolManager.GetActiveWaveManagers())
        {
            waveManager.HaltWave();
        }
    }

    public void ContinuePhase()
    {
        _isHalted = false;
        foreach (var waveManager in _wavePoolManager.GetActiveWaveManagers())
        {
            waveManager.ContinueWave();
        }
    }

    /// <summary>
    /// Logs debug messages consistently.
    /// </summary>
    private void LogDebug(string message) => Debug.Log($"PhaseManager: {message}");

    /// <summary>
    /// Logs error messages consistently.
    /// </summary>
    private void LogDebugError(string message) => Debug.LogError($"PhaseManager: {message}");
}
