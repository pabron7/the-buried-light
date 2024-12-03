using UnityEngine;

public class Charger : EnemyBase
{
    public override void Initialize(float speed, Vector3 direction, int health, int damage, bool isSpawner, EnemyTypes spawnType, int spawnCount, int spawnHealth, float spawnSpeed)
    {
        base.Initialize(speed, direction, health, damage, isSpawner, spawnType, spawnCount, spawnHealth, spawnSpeed);
    }

    public override void OnContact()
    {
        Debug.Log("Charger dealt damage!");
    }
}
