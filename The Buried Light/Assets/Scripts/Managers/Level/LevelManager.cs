using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using UniRx;

public class LevelManager : MonoBehaviour
{
    private ReactiveProperty<LevelStateBase> _currentState = new ReactiveProperty<LevelStateBase>();

    [SerializeField] private LevelConfig currentLevelConfig;
    private int _currentPhaseIndex = 0;

    private List<WaveManager> _waveManagerPool = new();
    private readonly List<WaveManager> _activeWaveManagers = new();

    public IReadOnlyReactiveProperty<LevelStateBase> CurrentState => _currentState;

    [Inject] private WaveManager.Factory _waveManagerFactory;
    [Inject] private GameManager _gameManager;

    private void Start()
    {
        Observable.EveryUpdate()
            .Where(_ => _gameManager.CurrentState.Value != null)
            .Take(1)
            .Subscribe(_ =>
            {
                Debug.Log("GameManager state ready. Subscribing to changes.");
                _gameManager.CurrentState
                    .Subscribe(OnGameManagerStateChanged)
                    .AddTo(this);
            })
            .AddTo(this);

        CreateWaveManagerPool(5); // Create a pool of WaveManagers
        SetState<IdleLevelState>();
    }

    private void CreateWaveManagerPool(int poolSize)
    {
        for (int i = 0; i < poolSize; i++)
        {
            var waveManager = _waveManagerFactory.Create();
            waveManager.gameObject.SetActive(false); // Initially deactivate
            _waveManagerPool.Add(waveManager);
        }
    }

    /// <summary>
    /// Changes the current state of the LevelManager.
    /// </summary>
    public void SetState<T>() where T : LevelStateBase
    {
        if (_currentState.Value is T)
        {
            Debug.LogWarning($"LevelManager is already in {typeof(T).Name} state.");
            return;
        }

        _currentState.Value?.OnStateExit();
        var newState = GetComponent<T>() ?? gameObject.AddComponent<T>();
        newState.OnStateEnter(this);
        _currentState.Value = newState;

        Debug.Log($"LevelManager state changed to: {typeof(T).Name}");
    }

    /// <summary>
    /// Starts the next phase in the level configuration.
    /// </summary>
    public void StartNextPhase()
    {
        if (_currentPhaseIndex >= currentLevelConfig.phases.Length)
        {
            Debug.Log("All phases completed.");
            SetState<CompletedLevelState>();
            return;
        }

        var currentPhase = currentLevelConfig.phases[_currentPhaseIndex];
        StartCoroutine(HandlePhase(currentPhase));
        _currentPhaseIndex++;
    }

    /// <summary>
    /// Handles a single phase by spawning and managing its waves.
    /// </summary>
    private IEnumerator HandlePhase(PhaseConfig phaseConfig)
    {
        Debug.Log($"Starting Phase {_currentPhaseIndex + 1}.");
        _activeWaveManagers.Clear();

        foreach (var waveConfig in phaseConfig.waves)
        {
            var waveManager = GetAvailableWaveManager();
            waveManager.Initialize(waveConfig);
            waveManager.OnWaveComplete += CheckPhaseProgress;
            waveManager.gameObject.SetActive(true);
            waveManager.StartWave();
            _activeWaveManagers.Add(waveManager);
        }

        // Wait until all waves in the phase are complete
        while (_activeWaveManagers.Exists(wm => wm.CurrentState != WaveManager.WaveState.WaveComplete))
        {
            yield return null;
        }

        Debug.Log($"Phase {_currentPhaseIndex + 1} completed.");
        StartNextPhase();
    }

    /// <summary>
    /// Checks the progress of the current phase and resets completed waves.
    /// </summary>
    private void CheckPhaseProgress()
    {
        if (_activeWaveManagers.TrueForAll(wm => wm.CurrentState == WaveManager.WaveState.WaveComplete))
        {
            foreach (var waveManager in _activeWaveManagers)
            {
                waveManager.ResetWave();
                ReturnWaveManagerToPool(waveManager);
            }
        }
    }

    /// <summary>
    /// Resets the wave manager pool for a new level or restart.
    /// </summary>
    public void ResetWavePool()
    {
        foreach (var waveManager in _activeWaveManagers)
        {
            waveManager.ResetWave();
            waveManager.gameObject.SetActive(false);
            _waveManagerPool.Add(waveManager);
        }
        _activeWaveManagers.Clear();
        Debug.Log("Wave manager pool has been reset.");
    }

    /// <summary>
    /// Checks if there are more phases to process.
    /// </summary>
    public bool HasMorePhases()
    {
        return _currentPhaseIndex < currentLevelConfig.phases.Length;
    }

    private WaveManager GetAvailableWaveManager()
    {
        if (_waveManagerPool.Count > 0)
        {
            var waveManager = _waveManagerPool[0];
            _waveManagerPool.RemoveAt(0);
            return waveManager;
        }

        Debug.LogError("No available WaveManager in the pool.");
        return null;
    }

    private void ReturnWaveManagerToPool(WaveManager waveManager)
    {
        waveManager.gameObject.SetActive(false);
        _waveManagerPool.Add(waveManager);
    }

    /// <summary>
    /// Handles state changes in the GameManager.
    /// </summary>
    private void OnGameManagerStateChanged(GameStateBase newState)
    {
        if (newState is PlayingState)
        {
            if (_currentState.Value is IdleLevelState || _currentState.Value is CompletedLevelState)
            {
                SetState<PreparingLevelState>();
            }
        }
        else if (newState is PausedState && _currentState.Value is InProgressLevelState)
        {
            SetState<IdleLevelState>();
        }
        else if (newState is MainMenuState)
        {
            SetState<IdleLevelState>();
        }
        else if (newState is GameOverState)
        {
            SetState<CompletedLevelState>();
        }
    }

    private void Update()
    {
        _currentState.Value?.OnUpdate();
    }
}
