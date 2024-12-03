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
        _currentWaveIndex = 0;
        StartNextWave();
    }

    public void StartNextWave()
    {
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
        for (int i = 0; i < waveConfig.enemyCount; i++)
        {
            _enemySpawner.SpawnEnemy(waveConfig);
            yield return new WaitForSeconds(waveConfig.spawnInterval);
        }

        _isWaveInProgress = false;
        _currentWaveIndex++;

        Debug.Log($"Wave {_currentWaveIndex} completed. Preparing next wave...");
        yield return new WaitForSeconds(2f);
        StartNextWave();
    }

}
