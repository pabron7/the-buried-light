using System.Collections;
using UnityEngine;
using Zenject;

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
                StartCoroutine(SpawnWave());
                break;

            case WaveState.WaveComplete:
                Debug.Log("WaveManager: Wave complete.");
                OnWaveComplete?.Invoke();
                SetState(WaveState.Idle); // Return to Idle
                break;
        }
    }

    private IEnumerator SpawnWave()
    {
        while (_spawnedEnemies < _waveConfig.enemyCount)
        {
            for (int i = 0; i < Mathf.Min(_waveConfig.groupSpawn ? _waveConfig.groupSize : 1, _remainingEnemies); i++)
            {
                var enemyObject = _enemySpawner.SpawnEnemy(_waveConfig);
                if (enemyObject.TryGetComponent<EnemyBase>(out var enemy))
                {
                    enemy.ConfigureOnDeathSpawn(
                        _waveConfig.spawnType,
                        _waveConfig.spawnCount,
                        _waveConfig.spawnHealth,
                        _waveConfig.spawnSpeed
                    );
                }

                _spawnedEnemies++;
                _remainingEnemies--;

                if (_spawnedEnemies >= _waveConfig.enemyCount)
                    break;
            }

            if (_remainingEnemies > 0)
            {
                yield return new WaitForSeconds(_waveConfig.spawnInterval);
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
