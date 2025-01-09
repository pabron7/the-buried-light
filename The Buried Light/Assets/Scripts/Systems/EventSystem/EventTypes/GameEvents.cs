using UniRx;
using System;

public class GameEvents : IGameEvents
{
    private readonly Subject<Unit> _onGameStarted = new Subject<Unit>();
    private readonly Subject<Unit> _onGameOver = new Subject<Unit>();
    private readonly Subject<Unit> _onLevelStart = new Subject<Unit>();
    private readonly Subject<int> _onPhaseStart = new Subject<int>();
    private readonly Subject<int> _onPhaseEnd = new Subject<int>();
    private readonly Subject<Unit> _onLevelEnd = new Subject<Unit>();
    private readonly Subject<Unit> _onLevelFail = new Subject<Unit>();
    private readonly Subject<Unit> _onMainMenu = new Subject<Unit>();
    private readonly Subject<Unit> _onPaused = new Subject<Unit>();
    private readonly Subject<Unit> _onResumed = new Subject<Unit>();
    private readonly Subject<Unit> _onTitleScreen = new Subject<Unit>();
    private readonly Subject<Unit> _onWaveComplete = new Subject<Unit>();
    private readonly Subject<string> _onLevelLoad = new Subject<string>();
    private readonly Subject<string> _onLevelRelease = new Subject<string>();

    public IObservable<Unit> OnGameStarted => _onGameStarted;
    public IObservable<Unit> OnGameOver => _onGameOver;
    public IObservable<Unit> OnLevelStart => _onLevelStart;
    public IObservable<int> OnPhaseStart => _onPhaseStart;
    public IObservable<int> OnPhaseEnd => _onPhaseEnd;
    public IObservable<Unit> OnLevelEnd => _onLevelEnd;
    public IObservable<Unit> OnLevelFail => _onLevelFail;
    public IObservable<Unit> OnMainMenu => _onMainMenu;
    public IObservable<Unit> OnPaused => _onPaused;
    public IObservable<Unit> OnResumed => _onResumed;
    public IObservable<Unit> OnTitleScreen => _onTitleScreen;
    public IObservable<Unit> OnWaveComplete => _onWaveComplete;
    public IObservable<string> OnLevelLoad => _onLevelLoad;
    public IObservable<string> OnLevelRelease => _onLevelRelease;

    public void NotifyGameStarted() => _onGameStarted.OnNext(Unit.Default);
    public void NotifyGameOver() => _onGameOver.OnNext(Unit.Default);
    public void NotifyLevelStart() => _onLevelStart.OnNext(Unit.Default);
    public void NotifyPhaseStart(int phaseIndex) => _onPhaseStart.OnNext(phaseIndex);
    public void NotifyPhaseEnd(int phaseIndex) => _onPhaseEnd.OnNext(phaseIndex);
    public void NotifyLevelEnd() => _onLevelEnd.OnNext(Unit.Default);
    public void NotifyLevelFail() => _onLevelFail.OnNext(Unit.Default);
    public void NotifyMainMenu() => _onMainMenu.OnNext(Unit.Default);
    public void NotifyPaused() => _onPaused.OnNext(Unit.Default);
    public void NotifyResumed() => _onResumed.OnNext(Unit.Default);
    public void NotifyTitleScreen() => _onTitleScreen.OnNext(Unit.Default);
    public void NotifyWaveComplete() => _onWaveComplete.OnNext(Unit.Default);
    public void NotifyLevelLoad(string levelName) => _onLevelLoad.OnNext(levelName);
    public void NotifyLevelRelease(string levelName) => _onLevelRelease.OnNext(levelName);
}
