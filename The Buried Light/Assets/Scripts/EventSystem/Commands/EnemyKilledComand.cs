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
        // Logic for handling enemy death (e.g., add score)
        Debug.Log($"Enemy {_enemy.Type} killed. Award points!");
    }
}
