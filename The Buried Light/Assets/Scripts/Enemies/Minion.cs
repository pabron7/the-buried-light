using UnityEngine;

public class Minion : EnemyBase
{
    private void Awake()
    {
        Initialize(EnemyTypes.minion, 0, 0, Vector3.zero);
    }

    public override void OnContact(int damage)
    {
        base.OnContact(damage);
        Debug.Log("Minion took damage.");
    }

    public override void OnDeath()
    {
        base.OnDeath();
        Debug.Log("Minion died.");
    }
}
