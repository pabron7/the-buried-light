using UnityEngine;

public class Champion : EnemyBase, ISpawner
{
    public int MinionsToSpawn { get; private set; }

    public void ConfigureOnDeathSpawn(EnemyTypes spawnType, int spawnCount, int spawnHealth, float spawnSpeed)
    {
  //
    }

    public override void Initialize(
        float speed,
        Vector3 direction,
        int health,
        int damage,
        bool isSpawner,
        EnemyTypes spawnType,
        int spawnCount,
        int spawnHealth,
        float spawnSpeed)
    {
        base.Initialize(speed, direction, health, damage, isSpawner, spawnType, spawnCount, spawnHealth, spawnSpeed);
    }

    protected override void OnDeath()
    {
        base.OnDeath();

    }

    public override void OnContact()
    {
        Debug.Log("Champion dealt damage!");
    }
}
