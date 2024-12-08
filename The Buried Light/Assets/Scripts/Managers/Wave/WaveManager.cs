using UnityEngine;
using System.Collections;
using Zenject;

public class WaveManager : MonoBehaviour
{
    private WaveConfig[] _waves;
    private EnemySpawner _enemySpawner;

    private int _currentWaveIndex;
    private bool _isWaveInProgress;

    public delegate void WaveManagerEvent();
    public event WaveManagerEvent OnAllWavesCompleted;

    [Inject]
    public void Construct(WaveConfig[] waves, EnemySpawner enemySpawner)
    {
        _waves = waves;
        _enemySpawner = enemySpawner;
    }

    private void Start()
    {
        Debug.Log($"WaveManager initialized with {_waves.Length} waves and EnemySpawner: {_enemySpawner}");
    }

    public void StartWaves()
    {
        if (_isWaveInProgress)
        {
            Debug.LogWarning("Waves already in progress!");
            return;
        }

        _currentWaveIndex = 0;
        StartNextWave();
    }

    public void StartNextWave()
    {
        if (_isWaveInProgress)
        {
            Debug.LogWarning("Wave is already in progress!");
            return;
        }

        if (_currentWaveIndex >= _waves.Length)
        {
            Debug.Log("All waves completed!");
            OnAllWavesCompleted?.Invoke();
            return;
        }

        _isWaveInProgress = true;

        WaveConfig currentWave = _waves[_currentWaveIndex];
        StartCoroutine(SpawnWave(currentWave));
    }

    private IEnumerator SpawnWave(WaveConfig waveConfig)
    {
        Debug.Log($"Starting wave {_currentWaveIndex + 1} with {waveConfig.enemyCount} enemies of type {waveConfig.enemyType}.");

        int spawnedEnemies = 0;

        while (spawnedEnemies < waveConfig.enemyCount)
        {
            int remainingEnemies = waveConfig.enemyCount - spawnedEnemies;
            int enemiesToSpawn = waveConfig.groupSpawn
                ? Mathf.Min(waveConfig.groupSize, remainingEnemies)
                : 1;

            Debug.Log($"Spawning a group of {enemiesToSpawn} enemies.");

            for (int i = 0; i < enemiesToSpawn; i++)
            {
                GameObject enemy = _enemySpawner.SpawnEnemy(waveConfig);

                if (enemy == null)
                {
                    Debug.LogWarning("Enemy pool empty. Waiting for pool recovery.");
                    yield return new WaitForSeconds(1f); // Wait for pool to recover
                    continue;
                }

                spawnedEnemies++;

                if (spawnedEnemies >= waveConfig.enemyCount)
                {
                    break; // Stop spawning if we've reached the limit
                }
            }

            Debug.Log($"Spawned {spawnedEnemies}/{waveConfig.enemyCount} enemies in wave {_currentWaveIndex + 1}.");

            if (spawnedEnemies < waveConfig.enemyCount)
            {
                yield return new WaitForSeconds(waveConfig.spawnInterval);
            }
        }

        Debug.Log($"Wave {_currentWaveIndex + 1} completed.");
        _isWaveInProgress = false;
        _currentWaveIndex++;

        yield return new WaitForSeconds(2f);

        StartNextWave();
    }
}
