using System.Collections;
using UnityEngine;
using Zenject;
using Cysharp.Threading.Tasks;

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

    /// <summary> The current phase index. </summary>
    public int CurrentPhaseIndex => _currentPhaseIndex;

    /// <summary>
    /// Determines if there are more phases to process in the level.
    /// </summary>
    public bool HasMorePhases()
    {
        return _currentPhaseIndex < _levelConfig.phases.Length;
    }

    /// <summary>
    /// Starts the next phase asynchronously and manages wave spawning.
    /// </summary>
    public async UniTask StartPhaseAsync()
    {
        if (!HasMorePhases())
        {
            LogDebug("No more phases available.");
            return;
        }

        LogDebug($"Starting Phase {_currentPhaseIndex + 1}");

        // Notify phase start
        _gameEvents.NotifyPhaseStart(_currentPhaseIndex + 1);
        var currentPhase = _levelConfig.phases[_currentPhaseIndex];
        _currentPhaseIndex++;

        await HandlePhaseAsync(currentPhase);

        // Notify phase end
        _gameEvents.NotifyPhaseEnd(_currentPhaseIndex);
    }

    /// <summary>
    /// Handles all waves in a phase asynchronously.
    /// </summary>
    private async UniTask HandlePhaseAsync(PhaseConfig phaseConfig)
    {
        var activeWaveManagers = _wavePoolManager.GetActiveWaveManagers();
        activeWaveManagers.Clear(); // Clear active managers at the start of the phase

        // Loop through each wave in the phase
        foreach (var waveConfig in phaseConfig.waves)
        {
            // Get a WaveManager instance from the pool
            var waveManager = _wavePoolManager.GetAvailableWaveManager();
            if (waveManager == null)
            {
                LogDebugError("Failed to retrieve WaveManager. Expanding pool dynamically.");
                waveManager = _wavePoolManager.CreateAndAddWaveManagerToPool(); // Dynamically create new instance
            }

            // Initialize and activate the WaveManager
            waveManager.Initialize(waveConfig);
            waveManager.gameObject.SetActive(true);
            waveManager.StartWave();
            activeWaveManagers.Add(waveManager);
        }

        // Wait until all active WaveManagers finish their waves
        await UniTask.WaitUntil(() =>
            activeWaveManagers.TrueForAll(wm => wm.CurrentState == WaveManager.WaveState.WaveComplete)
        );

        // Cleanup: Return all active WaveManagers to the pool
        foreach (var waveManager in activeWaveManagers)
        {
            waveManager.gameObject.SetActive(false); // Deactivate the WaveManager
            _wavePoolManager.ReturnWaveManagerToPool(waveManager);
        }

        activeWaveManagers.Clear(); // Clear active managers after phase completion
        LogDebug("Phase complete. All waves finished.");
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
    /// Halts or resumes the current phase and all active waves.
    /// </summary>
    public void TogglePhaseHalt()
    {
        _isHalted = !_isHalted;
        LogDebug(_isHalted ? "Halting phase." : "Continuing phase.");

        foreach (var waveManager in _wavePoolManager.GetActiveWaveManagers())
        {
            if (_isHalted)
                waveManager.HaltWave();
            else
                waveManager.ContinueWave();
        }
    }

    /// <summary>
    /// Cancels the current phase and stops all active waves.
    /// </summary>
    public void CancelPhase()
    {
        LogDebug("Cancelling phase.");
        _isHalted = false;

        foreach (var waveManager in _wavePoolManager.GetActiveWaveManagers())
        {
            waveManager.ResetWave();
        }
        _currentPhaseIndex = 0;
    }

    /// <summary>
    /// 
    /// </summary>
    public void ResetWaves()
    {
        LogDebug("Resetting all waves via PhaseManager.");
        _wavePoolManager.ResetPool();
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
