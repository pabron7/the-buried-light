using UnityEngine;
using UniRx;
using Zenject;
using Cysharp.Threading.Tasks;

public class GameManager : MonoBehaviour
{
    public ReactiveProperty<GameStateBase> CurrentState { get; private set; }
    public GameStateBase PreviousState { get; private set; }

    [Inject] private readonly DiContainer _container;
    [Inject] private GameEvents _gameEvents;

    private void Awake()
    {
        CurrentState = new ReactiveProperty<GameStateBase>(null);
     
    }

    private async void Start()
    {
        await UniTask.WaitUntil(() => _gameEvents != null);
    }

    /// <summary>
    /// Sets the game state using a generic type.
    /// </summary>
    public void SetState<T>() where T : GameStateBase
    {
        if (CurrentState.Value is T)
        {
            Debug.LogWarning($"Game is already in {typeof(T).Name} state.");
            return;
        }

        ChangeState(_container.Instantiate<T>());
    }

    /// <summary>
    /// Sets the game state using a state instance.
    /// </summary>
    public void SetState(GameStateBase newState)
    {
        if (CurrentState.Value == newState)
        {
            Debug.LogWarning($"Game is already in {newState.GetType().Name} state.");
            return;
        }

        ChangeState(newState);
    }

    private void ChangeState(GameStateBase newState)
    {
        // Set PreviousState before exiting the current state
        if (CurrentState.Value != null) 
        {    
            PreviousState = CurrentState.Value;
            CurrentState.Value.OnStateExit();
        }
        CurrentState.Value = newState;
        CurrentState.Value.OnStateEnter(this, _gameEvents);

        Debug.Log($"Game state changed to: {newState.GetType().Name}");
    }

    private void Update()
    {
        CurrentState.Value?.OnUpdate();
    }
}