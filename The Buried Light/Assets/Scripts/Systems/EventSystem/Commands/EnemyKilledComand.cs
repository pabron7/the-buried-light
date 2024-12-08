using UnityEngine;
public class EnemyKilledCommand : ICommand
{
    private readonly EnemyBase _enemy;

    public EnemyKilledCommand(EnemyBase enemy)
    {
        _enemy = enemy;
    }

    public void Execute()
    {
        Debug.Log($"Enemy {_enemy.Type} killed. Award points!");
    }
}
