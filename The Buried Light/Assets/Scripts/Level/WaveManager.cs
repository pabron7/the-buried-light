using UnityEngine;
using System.Collections;
using Zenject;

public class WaveManager : MonoBehaviour
{
    private WaveConfig[] _waves;
    private EnemySpawner _enemySpawner;

    private int _currentWaveIndex;
    private bool _isWaveInProgress;

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
            // Determine how many enemies to spawn in this batch
            int remainingEnemies = waveConfig.enemyCount - spawnedEnemies;
            int enemiesToSpawn = waveConfig.groupSpawn
                ? Mathf.Min(waveConfig.groupSize, remainingEnemies)
                : 1;

            // Spawn the group of enemies
            for (int i = 0; i < enemiesToSpawn; i++)
            {
                _enemySpawner.SpawnEnemy(waveConfig);
                spawnedEnemies++;

                if (spawnedEnemies >= waveConfig.enemyCount)
                {
                    break; // Stop spawning if we've reached the limit
                }
            }

            Debug.Log($"Spawned {spawnedEnemies}/{waveConfig.enemyCount} enemies in wave {_currentWaveIndex + 1}.");

            // Wait only if there are more enemies to spawn
            if (spawnedEnemies < waveConfig.enemyCount)
            {
                yield return new WaitForSeconds(waveConfig.spawnInterval);
            }
        }

        Debug.Log($"Wave {_currentWaveIndex + 1} completed.");
        _isWaveInProgress = false;
        _currentWaveIndex++;

        // Delay before starting the next wave
        yield return new WaitForSeconds(2f);

        StartNextWave();
    }

}
