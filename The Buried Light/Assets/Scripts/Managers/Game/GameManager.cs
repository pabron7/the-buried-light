using UnityEngine;
using UniRx;
using Zenject;

public class GameManager : MonoBehaviour
{
    public ReactiveProperty<GameStateBase> CurrentState { get; private set; }

    [Inject] private readonly DiContainer _container;

    private void Awake()
    {
        CurrentState = new ReactiveProperty<GameStateBase>(null);
    }

    private void Start()
    {
        SetState<TitleScreenState>();
    }

    public void SetState<T>() where T : GameStateBase
    {
        if (CurrentState.Value is T)
        {
            Debug.LogWarning($"Game is already in {typeof(T).Name} state.");
            return;
        }

        CurrentState.Value?.OnStateExit();
        CurrentState.Value = _container.Instantiate<T>();
        CurrentState.Value.OnStateEnter(this);

        Debug.Log($"Game state changed to: {typeof(T).Name}");
    }

    private void Update()
    {
        CurrentState.Value?.OnUpdate();
    }
}
