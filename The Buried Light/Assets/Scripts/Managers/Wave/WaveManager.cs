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

    public class Factory : PlaceholderFactory<WaveConfig, WaveManager>
    {
    }

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
    }

    private void Update()
    {
        // Add state logic in Update if needed for debugging or transitions
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
                _spawnedEnemies = 0;
                _remainingEnemies = _waveConfig.enemyCount;
                SetState(WaveState.Spawning);
                break;

            case WaveState.Spawning:
                StartCoroutine(SpawnWave());
                break;

            case WaveState.WaveComplete:
                Debug.Log("WaveManager: Wave complete.");
                OnWaveComplete?.Invoke();
                SetState(WaveState.Idle); // Return to Idle or let LevelManager decide
                break;
        }
    }

    private IEnumerator SpawnWave()
    {
        Debug.Log($"WaveManager: Spawning {_waveConfig.enemyCount} enemies of type {_waveConfig.enemyType}.");

        while (_spawnedEnemies < _waveConfig.enemyCount)
        {
            int enemiesToSpawn = Mathf.Min(_waveConfig.groupSpawn ? _waveConfig.groupSize : 1, _remainingEnemies);

            for (int i = 0; i < enemiesToSpawn; i++)
            {
                _enemySpawner.SpawnEnemy(_waveConfig);
                _spawnedEnemies++;
                _remainingEnemies--;

                if (_spawnedEnemies >= _waveConfig.enemyCount)
                {
                    break;
                }
            }

            Debug.Log($"WaveManager: Spawned {_spawnedEnemies}/{_waveConfig.enemyCount} enemies.");

            if (_remainingEnemies > 0)
            {
                yield return new WaitForSeconds(_waveConfig.spawnInterval);
            }
        }

        SetState(WaveState.WaveComplete);
    }
}
