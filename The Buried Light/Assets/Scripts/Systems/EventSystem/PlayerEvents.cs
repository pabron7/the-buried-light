using UniRx;
using UnityEngine;

public class PlayerEvents
{
    public Subject<Unit> OnPlayerShot { get; } = new Subject<Unit>();

    public void NotifyPlayerShot()
    {
        Debug.Log("Player shot.");
        OnPlayerShot.OnNext(Unit.Default);
    }
}
