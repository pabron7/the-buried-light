using System.Collections;
using UnityEngine;
using UniRx;
using Zenject;

public class LevelManager : MonoBehaviour
{
    private IdleLevelState _idleState;
    private PreparingLevelState _preparingState;
    private InProgressLevelState _inProgressState;
    private CompletedLevelState _completedState;

    private LevelStateBase _currentState;

    [Inject] private WavePoolManager _wavePoolManager;
    [Inject] private PhaseManager _phaseManager;
    [Inject] private GameManager _gameManager;
    [Inject] private GameEvents _gameEvents;
    [Inject] private DiContainer _container;

    public LevelStateBase CurrentState => _currentState;

    private void Awake()
    {
        // Preload states via Zenject
        _idleState = _container.Instantiate<IdleLevelState>();
        _preparingState = _container.Instantiate<PreparingLevelState>();
        _inProgressState = _container.Instantiate<InProgressLevelState>();
        _completedState = _container.Instantiate<CompletedLevelState>();

        Debug.Log("LevelManager: States preloaded.");
    }

    private void Start()
    {
        // React to GameManager state changes
        _gameManager.CurrentState
            .Subscribe(OnGameManagerStateChanged)
            .AddTo(this);

        // Initialize WaveManager pool
        _wavePoolManager.InitializePool(5);

        // Subscribe to game events for pausing, resuming, and canceling
        _gameEvents.OnPaused.Subscribe(_ => HaltLevel()).AddTo(this);
        _gameEvents.OnResumed.Subscribe(_ => ContinueLevel()).AddTo(this);
        _gameEvents.OnGameOver.Subscribe(_ => CancelLevel()).AddTo(this);

        SetState(_idleState);
    }

    /// <summary>
    /// Sets the current level state.
    /// </summary>
    public void SetState(LevelStateBase newState)
    {
        if (_currentState == newState)
        {
            Debug.LogWarning($"LevelManager: Already in {newState.GetType().Name} state.");
            return;
        }

        _currentState?.OnStateExit();
        _currentState = newState;
        _currentState.OnStateEnter(this);

        Debug.Log($"LevelManager: Transitioned to {newState.GetType().Name} state.");
    }

    /// <summary>
    /// Starts the next phase in the level.
    /// </summary>
    public void StartNextPhase()
    {
        if (!_phaseManager.HasMorePhases())
        {
            Debug.Log("LevelManager: All phases completed.");
            _gameEvents.NotifyLevelEnd();
            SetState(_completedState);
            return;
        }

        _gameEvents.NotifyPhaseStart(_phaseManager.CurrentPhaseIndex);
        StartCoroutine(_phaseManager.StartPhase());
    }

    /// <summary>
    /// Resets the WaveManager pool.
    /// </summary>
    public void ResetWavePool()
    {
        _wavePoolManager.ResetPool();
    }

    /// <summary>
    /// Handles state changes from the GameManager.
    /// </summary>
    private void OnGameManagerStateChanged(GameStateBase newState)
    {
        if (newState is PlayingState)
        {
            if (CurrentState is IdleLevelState || CurrentState is CompletedLevelState)
            {
                _gameEvents.NotifyLevelStart();
                SetState(_preparingState);
            }
        }
        else if (newState is GameOverState)
        {
            _gameEvents.NotifyLevelEnd();
            SetState(_completedState);
        }
    }

    /// <summary>
    /// Updates the current level state.
    /// </summary>
    private void Update()
    {
        _currentState?.OnUpdate();
    }

    /// <summary>
    /// Halts the current level progression and waves.
    /// </summary>
    private void HaltLevel()
    {
        Debug.Log("LevelManager: Halting level.");
        _phaseManager.HaltPhase();
    }

    /// <summary>
    /// Resumes the halted level progression and waves.
    /// </summary>
    private void ContinueLevel()
    {
        Debug.Log("LevelManager: Continuing level.");
        _phaseManager.ContinuePhase();
    }

    /// <summary>
    /// Cancels the current level and resets to idle state.
    /// </summary>
    private void CancelLevel()
    {
        Debug.Log("LevelManager: Cancelling level.");
        _phaseManager.CancelPhase();
        ResetWavePool();
        _gameEvents.NotifyLevelEnd();
        SetState(_idleState);
    }
}
