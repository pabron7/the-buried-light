using UnityEngine;

public class Charger : EnemyBase
{
    private void Awake()
    {
        Initialize(EnemyTypes.charger, 0, 0, Vector3.zero);
    }

    public override void OnContact(int damage)
    {
        base.OnContact(damage);
        Debug.Log("Charger took damage.");
    }

    public override void OnDeath()
    {
        base.OnDeath();
        Debug.Log("Charger died.");
    }
}
