using UnityEngine;

public class Charger : EnemyBase
{

    public override void OnDeath()
    {
        base.OnDeath();
        Debug.Log("Charger died.");
    }
}
