using System;

[Serializable]
public class ProjectileStats
{
    public float Speed { get; set; }
    public float LifeTime { get; set; }
    public int Damage { get; set; }
    public int Health { get; set; } 

    public ProjectileStats(float speed, float lifeTime, int damage, int health )
    {
        Speed = speed;
        LifeTime = lifeTime;
        Damage = damage;
        Health = health;
    }
}