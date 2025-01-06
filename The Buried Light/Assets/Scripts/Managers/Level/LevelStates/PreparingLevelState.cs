using UnityEngine;
using Zenject;

public class PreparingLevelState : LevelStateBase
{
    [Inject] GameEvents _gameEvents;

    public override void OnStateEnter(LevelManager levelManager)
    {
        base.OnStateEnter(levelManager);
        _gameEvents.NotifyLevelStart();
        levelManager.SetState(new InProgressLevelState());
    }
}
