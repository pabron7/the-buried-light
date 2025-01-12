using UnityEngine;
using UniRx;
using Zenject;
using Cysharp.Threading.Tasks;

public class LevelManager : MonoBehaviour
{
    [Inject] private IdleLevelState _idleState;
    [Inject] private PreparingLevelState _preparingState;
    [Inject] private InProgressLevelState _inProgressState;
    [Inject] private CompletedLevelState _completedState;
    [Inject] private FailedLevelState _failedLevelState;

    public FailedLevelState FailedLevelState => _failedLevelState;

    private LevelStateBase _currentState;
    public LevelStateBase CurrentState => _currentState;
    private int currentLevelIndex;

    [Inject] private PhaseManager _phaseManager;
    [Inject] private GameManager _gameManager;
    [Inject] private GameEvents _gameEvents;
    [Inject] private LevelLoader _levelLoader;
    [Inject] private GameProgressStore _gameProgressStore;

    public PhaseManager PhaseManager => _phaseManager;

    private int _totalPhases;
    private int _completedPhases;

    private bool _isLevelFailed = false;
    public bool IsLevelFailed
    {
        get => _isLevelFailed;
        set => _isLevelFailed = value;
    }


    private LevelData _currentLevelConfig;

    private async UniTaskVoid Start()
    {
        _gameManager.CurrentState
            .Subscribe(OnGameManagerStateChanged)
            .AddTo(this);

        SetState(_idleState);

        _gameEvents.OnPhaseEnd.Subscribe(_ => OnPhaseEnd()).AddTo(this);

        // Start loading the first level
        await LoadLevel(CurrentLevelAddress());
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
    /// Returns the current level address based on the current level in GameProgressStore.
    /// </summary>
    private string CurrentLevelAddress()
    {
        int currentLevel = _gameProgressStore.CurrentLevel.Value; // Get the current level
        return $"Level_{currentLevel}"; // Construct the address
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

        // Check if there are no more phases
        if (!_phaseManager.HasMorePhases())
        {
            SetState(_completedState);
            return;
        }

        // Start the next phase if conditions are met
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
                    SetState(_preparingState);
                }
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
