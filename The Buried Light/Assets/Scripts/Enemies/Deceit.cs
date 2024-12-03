using UnityEngine;

public class Deceit : EnemyBase, ISpawner
{
    public int ChampionsToSpawn { get; private set; }
    public float SpawnInterval { get; private set; }
    private float _lastSpawnTime;

    public void ConfigureOnDeathSpawn(EnemyTypes spawnType, int spawnCount, int spawnHealth, float spawnSpeed)
    {
//
    }

    public override void Initialize(float speed, Vector3 direction, int health, int damage, bool isSpawner, EnemyTypes spawnType, int spawnCount, int spawnHealth, float spawnSpeed)
    {
        base.Initialize(speed, direction, health, damage, isSpawner, spawnType, spawnCount, spawnHealth, spawnSpeed);
        ChampionsToSpawn = spawnCount;
        SpawnInterval = spawnSpeed;
    }

    private void Update()
    {
        base.Update();

 
    }

    public override void OnContact()
    {
        Debug.Log("Creature of Deceit dealt damage!");
    }
}
