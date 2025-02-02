using UnityEngine;
using Zenject;
using Cysharp.Threading.Tasks;

public class EnemySpawner : MonoBehaviour
{
    private EnemyFactory _enemyFactory;
    private EnemyPoolManager _enemyPoolManager;
    private GameFrame _gameFrame;

    [Inject]
    public void Construct(EnemyFactory enemyFactory, EnemyPoolManager enemyPoolManager, GameFrame gameFrame)
    {
        _enemyFactory = enemyFactory ?? throw new System.ArgumentNullException(nameof(enemyFactory));
        _enemyPoolManager = enemyPoolManager ?? throw new System.ArgumentNullException(nameof(enemyPoolManager));
        _gameFrame = gameFrame ?? throw new System.ArgumentNullException(nameof(gameFrame));
    }

    public async UniTask SpawnWave(WaveConfig waveConfig)
    {
        for (int i = 0; i < waveConfig.enemyCount; i++)
        {
            await SpawnEnemyAsync(waveConfig);
            await UniTask.Delay((int)(waveConfig.spawnInterval * 1000)); // Delay between spawns
        }
    }

    public async UniTask SpawnEnemyAsync(WaveConfig waveConfig)
    {
        if (_gameFrame == null)
        {
            Debug.LogWarning("EnemySpawner: GameFrame is null, skipping enemy spawn.");
            return;
        }

        Vector2 spawnPosition = _gameFrame.GetRandomPositionOutsideSpawnFrame();
        Vector2 targetPosition = _gameFrame.GetRandomPositionInsideFrame();
        Vector3 direction = (targetPosition - spawnPosition).normalized;

        GameObject enemyObject = await _enemyPoolManager.GetEnemyAsync(waveConfig.enemyType);

        if (enemyObject != null)
        {
            if (_gameFrame == null) return; // Prevent using destroyed GameFrame
            var enemy = enemyObject.GetComponent<EnemyBase>();
            if (enemy != null)
            {
                enemy.transform.position = spawnPosition;
                enemy.Initialize(waveConfig.enemyType, waveConfig.speed, waveConfig.health, direction);
            }
            else
            {
                Debug.LogError($"Prefab for {waveConfig.enemyType} does not contain an EnemyBase component!");
            }
        }
        else
        {
            Debug.LogError($"Failed to create enemy of type {waveConfig.enemyType}");
        }
    }




    public void ReturnEnemy(EnemyTypes type, GameObject enemy)
    {
        _enemyPoolManager.ReturnEnemy(type, enemy);
    }
}
