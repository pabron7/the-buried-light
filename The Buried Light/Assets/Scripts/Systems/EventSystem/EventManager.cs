using UniRx;
using UnityEngine;
using System;

public class EventManager
{
    public EnemyEvents EnemyEvents { get; }
    public PlayerEvents PlayerEvents { get; }
    public GameEvents GameEvents { get; }

    private readonly Subject<Unit> _onLevelStart = new Subject<Unit>();
    private readonly Subject<int> _onPhaseStart = new Subject<int>();
    private readonly Subject<int> _onPhaseEnd = new Subject<int>();
    private readonly Subject<Unit> _onLevelEnd = new Subject<Unit>();

    public IObservable<Unit> OnLevelStart => _onLevelStart; 
    public IObservable<int> OnPhaseStart => _onPhaseStart; 
    public IObservable<int> OnPhaseEnd => _onPhaseEnd; 
    public IObservable<Unit> OnLevelEnd => _onLevelEnd;

    public EventManager(EnemyEvents enemyEvents, PlayerEvents playerEvents, GameEvents gameEvents)
    {
        EnemyEvents = enemyEvents;
        PlayerEvents = playerEvents;
        GameEvents = gameEvents;
    }

    /// <summary>
    /// Triggers the level start event.
    /// </summary>
    public void PublishLevelStart()
    {
        _onLevelStart.OnNext(Unit.Default);
        Debug.Log("EventManager: Level start event triggered.");
    }

    /// <summary>
    /// Triggers the phase start event.
    /// </summary>
    public void PublishPhaseStart(int phaseIndex)
    {
        _onPhaseStart.OnNext(phaseIndex);
        Debug.Log($"EventManager: Phase {phaseIndex + 1} start event triggered.");
    }

    /// <summary>
    /// Triggers the phase end event.
    /// </summary>
    public void PublishPhaseEnd(int phaseIndex)
    {
        _onPhaseEnd.OnNext(phaseIndex);
        Debug.Log($"EventManager: Phase {phaseIndex + 1} end event triggered.");
    }

    /// <summary>
    /// Triggers the level end event.
    /// </summary>
    public void PublishLevelEnd()
    {
        _onLevelEnd.OnNext(Unit.Default);
        Debug.Log("EventManager: Level end event triggered.");
    }
}
