using UniRx;
using UnityEngine;
using System;

public class GameEvents : IGameEvents
{
    private readonly Subject<Unit> _onGameStarted = new Subject<Unit>();
    private readonly Subject<Unit> _onGameOver = new Subject<Unit>();
    private readonly Subject<Unit> _onLevelStart = new Subject<Unit>();
    private readonly Subject<int> _onPhaseStart = new Subject<int>();
    private readonly Subject<int> _onPhaseEnd = new Subject<int>();
    private readonly Subject<Unit> _onLevelEnd = new Subject<Unit>();

    public IObservable<Unit> OnGameStarted => _onGameStarted;
    public IObservable<Unit> OnGameOver => _onGameOver;
    public IObservable<Unit> OnLevelStart => _onLevelStart;
    public IObservable<int> OnPhaseStart => _onPhaseStart;
    public IObservable<int> OnPhaseEnd => _onPhaseEnd;
    public IObservable<Unit> OnLevelEnd => _onLevelEnd;

    public void NotifyGameStarted()
    {
        Debug.Log("Game started.");
        _onGameStarted.OnNext(Unit.Default);
    }

    public void NotifyGameOver()
    {
        Debug.Log("Game over.");
        _onGameOver.OnNext(Unit.Default);
    }

    public void NotifyLevelStart()
    {
        Debug.Log("Level started.");
        _onLevelStart.OnNext(Unit.Default);
    }

    public void NotifyPhaseStart(int phaseIndex)
    {
        Debug.Log($"Phase {phaseIndex} started.");
        _onPhaseStart.OnNext(phaseIndex);
    }

    public void NotifyPhaseEnd(int phaseIndex)
    {
        Debug.Log($"Phase {phaseIndex} ended.");
        _onPhaseEnd.OnNext(phaseIndex);
    }

    public void NotifyLevelEnd()
    {
        Debug.Log("Level ended.");
        _onLevelEnd.OnNext(Unit.Default);
    }
}