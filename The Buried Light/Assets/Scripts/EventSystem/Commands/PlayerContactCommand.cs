using UnityEngine;
public class PlayerContactCommand : ICommand
{
    private readonly EnemyBase _enemy;

    public PlayerContactCommand(EnemyBase enemy)
    {
        _enemy = enemy;
    }

    public void Execute()
    {
        // Logic for player damage or game updates
        Debug.Log($"Player contacted by enemy {_enemy.Type}. Apply damage!");
    }
}


