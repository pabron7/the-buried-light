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

    public class Factory : PlaceholderFactory<WaveManager> { }

    [Inject] private EnemySpawner _enemySpawner;

    private WaveConfig _waveConfig;
    private WaveState _currentState = WaveState.Idle;

    private int _spawnedEnemies;
    private int _remainingEnemies;

    public WaveState CurrentState => _currentState;

    public delegate void WaveEvent();
    public event WaveEvent OnWaveComplete;

    public void Initialize(WaveConfig waveConfig)
    {
        _waveConfig = waveConfig;
        _remainingEnemies = waveConfig.enemyCount;
        _spawnedEnemies = 0;
        _currentState = WaveState.Idle;
    }

    public void StartWave()
    {
        if (_currentState != WaveState.Idle)
        {
            Debug.LogWarning("WaveManager: Cannot start wave, not in Idle state.");
            return;
        }

        SetState(WaveState.WaveStart);
    }

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
                SpawnWaveAsync().Forget(); // Correct usage of Forget() for fire-and-forget
                break;

            case WaveState.WaveComplete:
                Debug.Log("WaveManager: Wave complete.");
                OnWaveComplete?.Invoke();
                SetState(WaveState.Idle); // Return to Idle
                break;
        }
    }

    private async UniTaskVoid SpawnWaveAsync()
    {
        SetState(WaveState.Spawning);

        while (_spawnedEnemies < _waveConfig.enemyCount)
        {
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

    public void ResetWave()
    {
        _waveConfig = null;
        _remainingEnemies = 0;
        _spawnedEnemies = 0;
        _currentState = WaveState.Idle;
    }
}
