using UniRx;
using UnityEngine;

public class GameEvents
{
    public Subject<Unit> OnGameStarted { get; } = new Subject<Unit>();
    public Subject<Unit> OnGameOver { get; } = new Subject<Unit>();

    public void NotifyGameStarted()
    {
        Debug.Log("Game started.");
        OnGameStarted.OnNext(Unit.Default);
    }

    public void NotifyGameOver()
    {
        Debug.Log("Game over.");
        OnGameOver.OnNext(Unit.Default);
    }
}
