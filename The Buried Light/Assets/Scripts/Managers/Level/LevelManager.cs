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
    public LevelStateBase CurrentState => _currentState;

    [Inject] private PhaseManager _phaseManager;
    [Inject] private GameManager _gameManager;
    [Inject] private GameEvents _gameEvents;
    [Inject] private DiContainer _container;
    [Inject] private LevelLoader _levelLoader;

    public PhaseManager PhaseManager => _phaseManager;

    private int _totalPhases;
    private int _completedPhases;

    private LevelConfig _currentLevelConfig;

    private void Awake()
    {
        // Preload states using Zenject
        _idleState = _container.Instantiate<IdleLevelState>();
        _preparingState = _container.Instantiate<PreparingLevelState>();
        _inProgressState = _container.Instantiate<InProgressLevelState>();
        _completedState = _container.Instantiate<CompletedLevelState>();

        LogDebug("States preloaded.");
    }

    private async UniTaskVoid Start()
    {
        _gameManager.CurrentState
            .Subscribe(OnGameManagerStateChanged)
            .AddTo(this);

        SetState(_idleState);

        _gameEvents.OnPhaseEnd.Subscribe(_ => OnPhaseEnd()).AddTo(this);

        // Start loading the first level
        await LoadLevel("Level_1");
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
    }

    /// <summary>
    /// Dynamically loads a LevelConfig asset using LevelLoader and sets it in PhaseManager.
    /// </summary>
    /// <param name="levelAddress">The Addressable address of the LevelConfig to load.</param>
    public async UniTask LoadLevel(string levelAddress)
    {
        _currentLevelConfig = await _levelLoader.LoadLevelAsync(levelAddress);
        if (_currentLevelConfig != null)
        {
            _phaseManager.SetLevelConfig(_currentLevelConfig); // Pass LevelConfig to PhaseManager
            LogDebug($"LevelManager: Loaded level {levelAddress}");
        }
        else
        {
            LogDebug($"LevelManager: Failed to load level {levelAddress}");
        }

        await UniTask.Delay(1000);
        SetState(_preparingState);
    }

    /// <summary>
    /// Releases the currently loaded level using LevelLoader.
    /// </summary>
    public void ReleaseCurrentLevel()
    {
        _levelLoader.ReleaseLevel();
        _currentLevelConfig = null;
    }

    /// <summary>
    /// Starts the next phase asynchronously.
    /// </summary>
    public async UniTaskVoid StartNextPhase()
    {
        if (!_phaseManager.HasMorePhases())
        {
            _gameEvents.NotifyLevelEnd();
            SetState(_completedState);
            return;
        }

        await _phaseManager.StartPhaseAsync();
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
    /// Handles the end of a phase and transitions to the next phase.
    /// </summary>
    private void OnPhaseEnd()
    {
        StartNextPhase().Forget();
    }

    /// <summary>
    /// Allows controlled access to set the GameManager's state.
    /// </summary>
    public void UpdateGameState<T>() where T : GameStateBase
    {
        _gameManager.SetState<T>();
    }

    public void UpdateGameState(GameStateBase newState)
    {
        _gameManager.SetState(newState);
    }

    /// <summary>
    /// Logs debug messages consistently.
    /// </summary>
    private void LogDebug(string message) => Debug.Log($"LevelManager: {message}");
}
