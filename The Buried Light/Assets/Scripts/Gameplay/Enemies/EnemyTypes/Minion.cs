using UnityEngine;

public class Minion : EnemyBase
{

    public override void OnDeath()
    {
        base.OnDeath();
        Debug.Log("Minion died.");
    }
}
