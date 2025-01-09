using UniRx;
using System;

public class PlayerEvents : IPlayerEvents
{
    private readonly Subject<Unit> _onPlayerShot = new Subject<Unit>();
    private readonly Subject<Unit> _onPlayerDeath = new Subject<Unit>();

    public IObservable<Unit> OnPlayerShot => _onPlayerShot;
    public IObservable<Unit> OnPlayerDeath => _onPlayerDeath;

    public void NotifyPlayerShot() => _onPlayerShot.OnNext(Unit.Default);
    public void NotifyPlayerDeath() => _onPlayerDeath.OnNext(Unit.Default);
}

