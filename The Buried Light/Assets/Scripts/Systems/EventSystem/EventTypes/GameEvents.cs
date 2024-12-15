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
    private readonly Subject<Unit> _onMainMenu = new Subject<Unit>();
    private readonly Subject<Unit> _onPaused = new Subject<Unit>();
    private readonly Subject<Unit> _onResumed = new Subject<Unit>();
    private readonly Subject<Unit> _onTitleScreen = new Subject<Unit>();

    public IObservable<Unit> OnGameStarted => _onGameStarted;
    public IObservable<Unit> OnGameOver => _onGameOver;
    public IObservable<Unit> OnLevelStart => _onLevelStart;
    public IObservable<int> OnPhaseStart => _onPhaseStart;
    public IObservable<int> OnPhaseEnd => _onPhaseEnd;
    public IObservable<Unit> OnLevelEnd => _onLevelEnd;
    public IObservable<Unit> OnMainMenu => _onMainMenu;
    public IObservable<Unit> OnPaused => _onPaused;
    public IObservable<Unit> OnResumed => _onResumed;
    public IObservable<Unit> OnTitleScreen => _onTitleScreen;

    public void NotifyGameStarted() => _onGameStarted.OnNext(Unit.Default);
    public void NotifyGameOver() => _onGameOver.OnNext(Unit.Default);
    public void NotifyLevelStart() => _onLevelStart.OnNext(Unit.Default);
    public void NotifyPhaseStart(int phaseIndex) => _onPhaseStart.OnNext(phaseIndex);
    public void NotifyPhaseEnd(int phaseIndex) => _onPhaseEnd.OnNext(phaseIndex);
    public void NotifyLevelEnd() => _onLevelEnd.OnNext(Unit.Default);
    public void NotifyMainMenu() => _onMainMenu.OnNext(Unit.Default);
    public void NotifyPaused() => _onPaused.OnNext(Unit.Default);
    public void NotifyResumed() => _onResumed.OnNext(Unit.Default);
    public void NotifyTitleScreen() => _onTitleScreen.OnNext(Unit.Default);
}
