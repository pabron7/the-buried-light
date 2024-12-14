using UniRx;
using UnityEngine;
using System;

public class PlayerEvents : IPlayerEvents
{
    private readonly Subject<Unit> _onPlayerShot = new Subject<Unit>();
    public IObservable<Unit> OnPlayerShot => _onPlayerShot;

    public void NotifyPlayerShot()
    {
        Debug.Log("Player shot.");
        _onPlayerShot.OnNext(Unit.Default);
    }
}
