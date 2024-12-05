using UnityEngine;
using Zenject;

public class Deceit : EnemyBase, ISpawner
{
    protected override void Move()
    {
        base.Move();
    }

    public override void OnDeath()
    {
        base.OnDeath();

        Debug.Log("Deceit died and spawned champions.");
    }

}
