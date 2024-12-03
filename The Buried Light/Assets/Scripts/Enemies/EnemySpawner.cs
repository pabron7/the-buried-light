using UnityEngine;
using Zenject;

public class EnemySpawner : MonoBehaviour
{
    private EnemyFactory _enemyFactory;
    private GameFrame _gameFrame;

    [Inject]
    public void Construct(EnemyFactory enemyFactory, GameFrame gameFrame)
    {
        _enemyFactory = enemyFactory ?? throw new System.ArgumentNullException(nameof(enemyFactory));
        _gameFrame = gameFrame ?? throw new System.ArgumentNullException(nameof(gameFrame));
    }

    public void SpawnEnemy(WaveConfig waveConfig)
    {
        for (int i = 0; i < waveConfig.enemyCount; i++)
        {
            SpawnSingleEnemy(waveConfig);
        }
    }

    private void SpawnSingleEnemy(WaveConfig waveConfig)
    {
        if (_enemyFactory == null)
        {
            Debug.LogError("_enemyFactory is null. Ensure it is injected properly.");
            return;
        }

        Vector2 spawnPosition = _gameFrame.GetRandomPositionOutsideFrame();
        Vector2 directionTarget = _gameFrame.GetRandomPositionInsideFrame();
        Vector3 direction = directionTarget - (Vector2)spawnPosition;

        EnemyBase enemy = _enemyFactory.Create(waveConfig.enemyType);
        if (enemy != null)
        {
            enemy.transform.position = spawnPosition;
            enemy.Initialize(waveConfig.enemyType, waveConfig.speed, waveConfig.health, direction);
        }
        else
        {
            Debug.LogError($"Failed to create enemy of type {waveConfig.enemyType}");
        }
    }
}
