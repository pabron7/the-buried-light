using UnityEngine;
using Zenject;

public class GameManager : MonoBehaviour
{
    private GameStateBase _currentState;

    public GameStateBase CurrentState => _currentState;

    [Inject] private readonly DiContainer _container;

    public void SetState<T>() where T : GameStateBase, new()
    {
        if (_currentState is T)
        {
            Debug.LogWarning($"Game is already in {typeof(T).Name} state.");
            return;
        }

        _currentState?.OnStateExit();
        _currentState = new T(); 
        _currentState.OnStateEnter(this);

        Debug.Log($"Game state changed to: {typeof(T).Name}");
    }

    private void Start()
    {
        SetState<TitleScreenState>();
    }

    private void Update()
    {
        _currentState?.OnUpdate();
    }
}
