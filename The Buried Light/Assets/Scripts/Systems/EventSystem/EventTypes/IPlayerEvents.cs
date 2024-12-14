using UniRx;
using System;

public interface IPlayerEvents
{
    IObservable<Unit> OnPlayerShot { get; }

    void NotifyPlayerShot();
}
