using UnityEngine;
using Zenject;

public class Champion : EnemyBase, ISpawner
{

    public override void OnDeath()
    {
        base.OnDeath();

        Debug.Log("Champion died and spawned minions.");

    }
}
