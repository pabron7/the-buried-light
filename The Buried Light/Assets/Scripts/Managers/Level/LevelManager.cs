using UnityEngine;
using UniRx;
using Zenject;

public class LevelManager : MonoBehaviour
{
    private int _currentPhaseIndex = 0;

    private IdleLevelState _idleState;
    private PreparingLevelState _preparingState;
    private InProgressLevelState _inProgressState;
    private CompletedLevelState _completedState;

    private LevelStateBase _currentState;

    [Inject] private WavePoolManager _wavePoolManager;
    [Inject] private PhaseManager _phaseManager;
    [Inject] private GameManager _gameManager;
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
        // Optionally react to GameManager state changes
        _gameManager.CurrentState
            .Subscribe(OnGameManagerStateChanged)
            .AddTo(this);

        _wavePoolManager.InitializePool(5); // Initialize WaveManager pool
        SetState(_idleState);
    }

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

    public void StartNextPhase()
    {
        if (!_phaseManager.HasMorePhases())
        {
            Debug.Log("All phases completed.");
            SetState(_completedState);
            return;
        }

        StartCoroutine(_phaseManager.StartPhase());
    }

    public void ResetWavePool()
    {
        _wavePoolManager.ResetPool();
    }

    private void OnGameManagerStateChanged(GameStateBase newState)
    {
        if (newState is PlayingState)
        {
            if (CurrentState is IdleLevelState || CurrentState is CompletedLevelState)
            {
                SetState(_preparingState);
            }
        }
        else if (newState is PausedState && CurrentState is InProgressLevelState)
        {
            SetState(_idleState);
        }
        else if (newState is MainMenuState)
        {
            SetState(_idleState);
        }
        else if (newState is GameOverState)
        {
            SetState(_completedState);
        }
    }

    private void Update()
    {
        _currentState?.OnUpdate();
    }
}
