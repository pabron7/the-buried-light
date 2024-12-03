using UnityEngine;
using Zenject;

public class Champion : EnemyBase, ISpawner
{

    private void Awake()
    {
        Initialize(EnemyTypes.champion, 0, 0, Vector3.zero);
    }

    public override void OnDeath()
    {
        base.OnDeath();

        Debug.Log("Champion died and spawned minions.");

    }

    public override void OnContact(int damage)
    {
        base.OnContact(damage);

        Debug.Log("Champion took damage.");
    }
}
