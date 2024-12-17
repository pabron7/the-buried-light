using System.Collections;
using UnityEngine;
using UniRx;
using Zenject;
using Cysharp.Threading.Tasks;

public class LevelManager : MonoBehaviour
{
    private IdleLevelState _idleState;
    private PreparingLevelState _preparingState;
    private InProgressLevelState _inProgressState;
    private CompletedLevelState _completedState;

    private LevelStateBase _currentState;

    [Inject] private PhaseManager _phaseManager;
    [Inject] private GameManager _gameManager;
    [Inject] private GameEvents _gameEvents;
    [Inject] private DiContainer _container;

    public LevelStateBase CurrentState => _currentState;

    public PhaseManager PhaseManager => _phaseManager;

    private void Awake()
    {
        // Preload states using Zenject
        _idleState = _container.Instantiate<IdleLevelState>();
        _preparingState = _container.Instantiate<PreparingLevelState>();
        _inProgressState = _container.Instantiate<InProgressLevelState>();
        _completedState = _container.Instantiate<CompletedLevelState>();

        LogDebug("States preloaded.");
    }

    private void Start()
    {
        // Subscribe to GameManager state changes
        _gameManager.CurrentState
            .Subscribe(OnGameManagerStateChanged)
            .AddTo(this);

        SetState(_idleState);
    }

    /// <summary>
    /// Sets the current level state if it's not already set.
    /// </summary>
    public void SetState(LevelStateBase newState)
    {
        if (_currentState == newState) return;

        _currentState?.OnStateExit();
        _currentState = newState;
        _currentState.OnStateEnter(this);

        LogDebug($"Transitioned to {newState.GetType().Name}");
    }

    /// <summary>
    /// Starts the next phase asynchronously.
    /// </summary>
    public async UniTaskVoid StartNextPhase()
    {
        if (!_phaseManager.HasMorePhases())
        {
            LogDebug("All phases completed.");
            _gameEvents.NotifyLevelEnd();
            SetState(_completedState);
            return;
        }

        await _phaseManager.StartPhaseAsync();
        LogDebug("Phase completed, ready for next phase.");
    }

    /// <summary>
    /// Handles state changes from the GameManager.
    /// </summary>
    private void OnGameManagerStateChanged(GameStateBase newState)
    {
        switch (newState)
        {
            case PlayingState:
                if (_currentState is IdleLevelState or CompletedLevelState)
                {
                    _gameEvents.NotifyLevelStart();
                    SetState(_preparingState);
                }
                break;

            case GameOverState:
                _gameEvents.NotifyLevelEnd();
                SetState(_completedState);
                break;
        }
    }

    /// <summary>
    /// Updates the current level state every frame.
    /// </summary>
    private void Update()
    {
        _currentState?.OnUpdate();
    }

    /// <summary>
    /// Logs debug messages consistently.
    /// </summary>
    private void LogDebug(string message) => Debug.Log($"LevelManager: {message}");
}
