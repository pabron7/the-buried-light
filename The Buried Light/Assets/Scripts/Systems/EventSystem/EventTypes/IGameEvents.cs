using UniRx;
using UnityEngine;
using System;

public interface IGameEvents
{
    IObservable<Unit> OnGameStarted { get; }
    IObservable<Unit> OnGameOver { get; }
    IObservable<Unit> OnLevelStart { get; }
    IObservable<int> OnPhaseStart { get; }
    IObservable<int> OnPhaseEnd { get; }
    IObservable<Unit> OnLevelEnd { get; }

    void NotifyGameStarted();
    void NotifyGameOver();
    void NotifyLevelStart();
    void NotifyPhaseStart(int phaseIndex);
    void NotifyPhaseEnd(int phaseIndex);
    void NotifyLevelEnd();
}