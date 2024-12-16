using UnityEngine;
using Zenject;
using Cysharp.Threading.Tasks;

public class WaveManager : MonoBehaviour
{
    public enum WaveState
    {
        Idle,
        WaveStart,
        Spawning,
        WaveComplete
    }

    private bool _isHalted = false;

    public class Factory : PlaceholderFactory<WaveManager> { }

    [Inject] private EnemySpawner _enemySpawner;

    private WaveConfig _waveConfig;
    private WaveState _currentState = WaveState.Idle;

    private int _spawnedEnemies;
    private int _remainingEnemies;

    public WaveState CurrentState => _currentState;

    public delegate void WaveEvent();
    public event WaveEvent OnWaveComplete;

    /// <summary>
    /// Initializes the wave manager with a specific wave configuration.
    /// Resets internal states and prepares the manager for spawning enemies.
    /// </summary>
    public void Initialize(WaveConfig waveConfig)
    {
        _waveConfig = waveConfig;
        _remainingEnemies = waveConfig.enemyCount;
        _spawnedEnemies = 0;
        _currentState = WaveState.Idle;
        _isHalted = false;
    }

    /// <summary>
    /// Starts the wave spawning process if the manager is in the Idle state.
    /// </summary>
    public void StartWave()
    {
        if (_currentState != WaveState.Idle)
        {
            Debug.LogWarning("WaveManager: Cannot start wave, not in Idle state.");
            return;
        }

        SetState(WaveState.WaveStart);
    }

    /// <summary>
    /// Sets the current state of the wave manager and triggers related actions.
    /// </summary>
    private void SetState(WaveState newState)
    {
        _currentState = newState;

        switch (newState)
        {
            case WaveState.Idle:
                Debug.Log("WaveManager: Entering Idle state.");
                break;

            case WaveState.WaveStart:
                Debug.Log("WaveManager: Starting wave.");
                SpawnWaveAsync().Forget(); // Fire-and-forget spawning logic
                break;

            case WaveState.WaveComplete:
                Debug.Log("WaveManager: Wave complete.");
                OnWaveComplete?.Invoke();
                SetState(WaveState.Idle); // Return to Idle state
                break;
        }
    }

    /// <summary>
    /// Spawns enemies asynchronously based on the wave configuration.
    /// Supports group spawning and handles delays between spawns.
    /// </summary>
    private async UniTaskVoid SpawnWaveAsync()
    {
        SetState(WaveState.Spawning);

        while (_spawnedEnemies < _waveConfig.enemyCount)
        {
            if (_isHalted)
            {
                await UniTask.Yield(); // Wait until resumed
                continue;
            }

            for (int i = 0; i < Mathf.Min(_waveConfig.groupSpawn ? _waveConfig.groupSize : 1, _remainingEnemies); i++)
            {
                await _enemySpawner.SpawnEnemyAsync(_waveConfig); // Async spawning
                _spawnedEnemies++;
                _remainingEnemies--;

                if (_spawnedEnemies >= _waveConfig.enemyCount)
                    break;
            }

            if (_remainingEnemies > 0)
            {
                await UniTask.Delay((int)(_waveConfig.spawnInterval * 1000)); // Delay between groups
            }
        }

        SetState(WaveState.WaveComplete);
    }

    /// <summary>
    /// Resets the wave manager to its initial state, stopping any ongoing actions.
    /// </summary>
    public void ResetWave()
    {
        _waveConfig = null;
        _remainingEnemies = 0;
        _spawnedEnemies = 0;
        _currentState = WaveState.Idle;
        _isHalted = false;
    }

    /// <summary>
    /// Halts the wave spawning process if it is in progress.
    /// </summary>
    public void HaltWave()
    {
        if (_currentState == WaveState.Spawning)
        {
            Debug.Log("WaveManager: Halting wave spawning.");
            _isHalted = true;
        }
    }

    /// <summary>
    /// Resumes the wave spawning process if it was halted.
    /// </summary>
    public void ContinueWave()
    {
        if (_isHalted)
        {
            Debug.Log("WaveManager: Continuing wave spawning.");
            _isHalted = false;
        }
    }

    /// <summary>
    /// Cancels the current wave, stopping all actions and resetting the manager.
    /// </summary>
    public void CancelWave()
    {
        if (_currentState == WaveState.Spawning || _currentState == WaveState.WaveStart)
        {
            Debug.Log("WaveManager: Cancelling wave.");
            ResetWave(); // Reset the wave and stop spawning
        }
    }
}
