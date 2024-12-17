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
    /// Starts the wave spawning process if in the Idle state.
    /// </summary>
    public void StartWave()
    {
        if (_currentState != WaveState.Idle)
            return;

        SetState(WaveState.WaveStart);
    }

    /// <summary>
    /// Handles state changes and triggers corresponding actions.
    /// </summary>
    private void SetState(WaveState newState, bool logStateChange = true)
    {
        _currentState = newState;

        if (logStateChange)
            Debug.Log($"WaveManager: State changed to {newState}");

        switch (newState)
        {
            case WaveState.WaveStart:
                SpawnWaveAsync().Forget(); // Fire-and-forget spawning
                break;

            case WaveState.WaveComplete:
                OnWaveComplete?.Invoke();
                ResetWave();
                break;
        }
    }

    /// <summary>
    /// Spawns enemies asynchronously based on wave configuration.
    /// </summary>
    private async UniTaskVoid SpawnWaveAsync()
    {
        SetState(WaveState.Spawning);

        while (_spawnedEnemies < _waveConfig.enemyCount)
        {
            if (_isHalted)
            {
                await UniTask.Yield(); // Pause if halted
                continue;
            }

            var enemiesToSpawn = Mathf.Min(_waveConfig.groupSpawn ? _waveConfig.groupSize : 1, _remainingEnemies);

            for (int i = 0; i < enemiesToSpawn; i++)
            {
                await _enemySpawner.SpawnEnemyAsync(_waveConfig);
                _spawnedEnemies++;
                _remainingEnemies--;

                if (_spawnedEnemies >= _waveConfig.enemyCount)
                    break;
            }

            if (_remainingEnemies > 0)
                await UniTask.Delay((int)(_waveConfig.spawnInterval * 1000)); // Delay between groups
        }

        SetState(WaveState.WaveComplete);
    }

    /// <summary>
    /// Resets the wave manager to its initial state.
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
    /// Halts the wave spawning process if active.
    /// </summary>
    public void HaltWave()
    {
        if (_currentState == WaveState.Spawning)
        {
            _isHalted = true;
            Debug.Log("WaveManager: Wave spawning halted.");
        }
    }

    /// <summary>
    /// Resumes the wave spawning process if halted.
    /// </summary>
    public void ContinueWave()
    {
        if (_isHalted)
        {
            _isHalted = false;
            Debug.Log("WaveManager: Wave spawning resumed.");
        }
    }
}
