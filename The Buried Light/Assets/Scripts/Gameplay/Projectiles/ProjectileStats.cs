using System;

[Serializable]
public class ProjectileStats
{
    public float Speed { get; set; }
    public float LifeTime { get; set; }
    public int Damage { get; set; }

    public ProjectileStats(float speed, float lifeTime, int damage)
    {
        Speed = speed;
        LifeTime = lifeTime;
        Damage = damage;
    }
}
