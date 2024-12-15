using UnityEngine;
using UniRx;
using Zenject;
using Cysharp.Threading.Tasks;

public class GameManager : MonoBehaviour
{
    public ReactiveProperty<GameStateBase> CurrentState { get; private set; }

    [Inject] private readonly DiContainer _container;
    [Inject] private GameEvents _gameEvents;

    private void Awake()
    {
        CurrentState = new ReactiveProperty<GameStateBase>(null);
    }

    private async void Start()
    {

        await UniTask.WaitUntil(() => _gameEvents != null);

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
        CurrentState.Value.OnStateEnter(this, _gameEvents);

        Debug.Log($"Game state changed to: {typeof(T).Name}");
    }

    private void Update()
    {
        CurrentState.Value?.OnUpdate();
    }
}

