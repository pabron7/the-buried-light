using UnityEngine;

public class EnemyDamageCommand : ICommand
{
    private readonly IKillable _target;
    private readonly int _damage;

    public EnemyDamageCommand(IKillable target, int damage)
    {
        _target = target;
        _damage = damage;
    }

    public void Execute()
    {
        if (_target == null)
        {
            Debug.LogError("EnemyDamageCommand: Target is null. Command execution aborted.");
            return;
        }

        if (_damage <= 0)
        {
            Debug.LogWarning("EnemyDamageCommand: Damage value must be greater than 0.");
            return;
        }

        _target.TakeDamage(_damage);
        Debug.Log($"EnemyDamageCommand: {_damage} damage applied to {_target}");
    }
}