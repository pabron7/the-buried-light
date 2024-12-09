using UnityEngine;
using Zenject;

public class GameManager : MonoBehaviour
{
    private GameStateBase _currentState;

    public GameStateBase CurrentState => _currentState;

    [Inject] private readonly DiContainer _container;

    public void SetState<T>() where T : GameStateBase
    {
        if (_currentState is T)
        {
            Debug.LogWarning($"Game is already in {typeof(T).Name} state.");
            return;
        }

        _currentState?.OnStateExit();
        _currentState = _container.Instantiate<T>();
        _currentState.OnStateEnter(this);

        Debug.Log($"Game state changed to: {typeof(T).Name}");
    }

    private void Start()
    {
        SetState<MainMenuState>(); 
    }

    private void Update()
    {
        _currentState?.OnUpdate();
    }
}
