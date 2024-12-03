using UnityEngine;
using Zenject;

public class Deceit : EnemyBase, ISpawner
{
    private void Awake()
    {
        Initialize(EnemyTypes.deceit, 0, 0, Vector3.zero);
    }

    protected override void Move()
    {
        base.Move();
    }

    public override void OnDeath()
    {
        base.OnDeath();

        Debug.Log("Deceit died and spawned champions.");
    }

    public override void OnContact(int damage)
    {
        base.OnContact(damage);

        Debug.Log("Deceit took damage.");
    }
}
