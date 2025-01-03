using UniRx;
using UnityEngine;
using System;

public class EventManager
{
    public EnemyEvents EnemyEvents { get; }
    public PlayerEvents PlayerEvents { get; }
    public GameEvents GameEvents { get; }

    public EventManager(EnemyEvents enemyEvents, PlayerEvents playerEvents, GameEvents gameEvents)
    {
        EnemyEvents = enemyEvents;
        PlayerEvents = playerEvents;
        GameEvents = gameEvents;
    }

}
