using UniRx;
using Zenject;
using UnityEngine;

public class LevelResult
{
    private readonly GameEvents _gameEvents;

    public ReactiveProperty<bool> IsLevelFailed { get; private set; } = new ReactiveProperty<bool>(false);
    public ReactiveProperty<bool> IsLevelCompleted { get; private set; } = new ReactiveProperty<bool>(false);

    [Inject]
    public LevelResult(GameEvents gameEvents)
    {
        _gameEvents = gameEvents;
    }

    public void SetFailed()
    {
        IsLevelFailed.Value = true;
        
        Debug.Log("Level failed.");
    }

    public void SetCompleted()
    {
        IsLevelCompleted.Value = true;
  
        Debug.Log("Level completed.");
    }

    public void Reset()
    {
        IsLevelFailed.Value = false;
        IsLevelCompleted.Value = false;
    }
}
