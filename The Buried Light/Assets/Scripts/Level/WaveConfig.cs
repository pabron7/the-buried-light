using UnityEngine;

[CreateAssetMenu(fileName = "WaveConfig", menuName = "Game/WaveConfig")]
public class WaveConfig : ScriptableObject
{
    [Header("Base Stats")]
    public EnemyTypes enemyType;
    public int enemyCount;
    public float speed;
    public int health;

    [Header("Time")]
    [Tooltip("Declare the density of spawns. !!!This is not related to on death spawns!!! The spawner will wait for this amount of time between 2 spawns.")]
    public float spawnInterval;

    [Header("Group")]
    [Tooltip("Check this box if the monsters should be spawned within groups rather than initially spawning them.")]
    public bool groupSpawn;
    [Tooltip("Simply declare the size of groups. Declared number of monsters will be periodically spawned according to the spawnInternal value.")]
    public int groupSize;

    [Header("On Death Stats")]
    [Tooltip("If the monster is not spawning anything when it dies, leave the below spawn fields as they are.")]
    public bool isSpawner;
    public EnemyTypes spawnType;
    public int spawnCount;
    public int spawnHealth;
    public float spawnSpeed;
}