using System.Collections.Generic;
using UnityEngine;
using Zenject;
using UniRx;
using Cysharp.Threading.Tasks;

public class LevelManager : MonoBehaviour
{
    private ReactiveProperty<LevelStateBase> _currentState = new ReactiveProperty<LevelStateBase>();

    [SerializeField] private LevelConfig currentLevelConfig;
    private int _currentPhaseIndex = 0;

    private Queue<WaveManager> _waveManagerPool = new(); // Optimized to use a queue
    private readonly List<WaveManager> _activeWaveManagers = new();

    public IReadOnlyReactiveProperty<LevelStateBase> CurrentState => _currentState;

    [Inject] private WaveManager.Factory _waveManagerFactory;
    [Inject] private GameManager _gameManager;

    /// <summary>
    /// Initializes the LevelManager by waiting for the GameManager state and creating a WaveManager pool.
    /// </summary>
    private async void Start()
    {
        await UniTask.WaitUntil(() => _gameManager.CurrentState.Value != null);
        Debug.Log("GameManager state ready. Subscribing to changes.");

        _gameManager.CurrentState
            .Subscribe(OnGameManagerStateChanged)
            .AddTo(this);

        await CreateWaveManagerPoolAsync(5); // Initialize a pool of WaveManagers
        await SetStateAsync<IdleLevelState>();
    }

    /// <summary>
    /// Creates a pool of WaveManagers asynchronously to avoid blocking the main thread.
    /// </summary>
    private async UniTask CreateWaveManagerPoolAsync(int poolSize)
    {
        for (int i = 0; i < poolSize; i++)
        {
            var waveManager = _waveManagerFactory.Create();
            waveManager.gameObject.SetActive(false);
            _waveManagerPool.Enqueue(waveManager);
            await UniTask.Yield();
        }
        Debug.Log($"WaveManager pool created with {poolSize} managers.");
    }

    /// <summary>
    /// Sets the current level state asynchronously and ensures proper transitions.
    /// </summary>
    public async UniTask SetStateAsync<T>() where T : LevelStateBase, new()
    {
        if (_currentState.Value is T)
        {
            Debug.LogWarning($"LevelManager is already in {typeof(T).Name} state.");
            return;
        }

        if (_currentState.Value != null)
        {
            await _currentState.Value.OnStateExitAsync();
        }

        var newState = GetOrCreateState<T>();
        await newState.OnStateEnterAsync(this);
        _currentState.Value = newState;

        Debug.Log($"LevelManager state changed to: {typeof(T).Name}");
    }

    /// <summary>
    /// Retrieves or creates a new state instance of the specified type.
    /// </summary>
    private T GetOrCreateState<T>() where T : LevelStateBase, new()
    {
        return GetComponent<T>() ?? gameObject.AddComponent<T>();
    }

    /// <summary>
    /// Starts the next phase of the level asynchronously.
    /// </summary>
    public async UniTask StartNextPhaseAsync()
    {
        if (_currentPhaseIndex >= currentLevelConfig.phases.Length)
        {
            Debug.Log("All phases completed.");
            await SetStateAsync<CompletedLevelState>();
            return;
        }

        var currentPhase = currentLevelConfig.phases[_currentPhaseIndex];
        await HandlePhaseAsync(currentPhase);
        _currentPhaseIndex++;
    }

    /// <summary>
    /// Handles a single phase by spawning and managing its waves asynchronously.
    /// </summary>
    private async UniTask HandlePhaseAsync(PhaseConfig phaseConfig)
    {
        Debug.Log($"Starting Phase {_currentPhaseIndex + 1}.");
        _activeWaveManagers.Clear();

        foreach (var waveConfig in phaseConfig.waves)
        {
            var waveManager = GetAvailableWaveManager();
            waveManager.Initialize(waveConfig);
            waveManager.OnWaveComplete += CheckPhaseProgress; // Subscribe to the event
            waveManager.gameObject.SetActive(true);
            waveManager.StartWave();
            _activeWaveManagers.Add(waveManager);
        }

        await UniTask.WaitUntil(() =>
            _activeWaveManagers.TrueForAll(wm => wm.CurrentState == WaveManager.WaveState.WaveComplete));

        Debug.Log($"Phase {_currentPhaseIndex + 1} completed.");
        await StartNextPhaseAsync();
    }

    /// <summary>
    /// Checks the progress of the current phase and returns completed waves to the pool.
    /// </summary>
    private void CheckPhaseProgress()
    {
        for (int i = _activeWaveManagers.Count - 1; i >= 0; i--)
        {
            var waveManager = _activeWaveManagers[i];
            if (waveManager.CurrentState == WaveManager.WaveState.WaveComplete)
            {
                waveManager.ResetWave();
                ReturnWaveManagerToPool(waveManager);
                _activeWaveManagers.RemoveAt(i);
            }
        }

        if (_activeWaveManagers.Count == 0)
        {
            Debug.Log("All waves in the phase are complete.");
        }
    }


    /// <summary>
    /// Resets the wave pool by deactivating all active WaveManagers.
    /// </summary>
    public void ResetWavePool()
    {
        foreach (var waveManager in _activeWaveManagers)
        {
            waveManager.ResetWave();
            waveManager.gameObject.SetActive(false);
            _waveManagerPool.Enqueue(waveManager);
        }
        _activeWaveManagers.Clear();
        Debug.Log("Wave manager pool has been reset.");
    }

    /// <summary>
    /// Checks if there are more phases left in the current level configuration.
    /// </summary>
    public bool HasMorePhases()
    {
        return _currentPhaseIndex < currentLevelConfig.phases.Length;
    }

    /// <summary>
    /// Retrieves an available WaveManager from the pool.
    /// </summary>
    private WaveManager GetAvailableWaveManager()
    {
        if (_waveManagerPool.Count > 0)
        {
            return _waveManagerPool.Dequeue();
        }

        Debug.LogError("No available WaveManager in the pool.");
        return null;
    }

    /// <summary>
    /// Returns a WaveManager to the pool after deactivating it.
    /// </summary>
    private void ReturnWaveManagerToPool(WaveManager waveManager)
    {
        waveManager.gameObject.SetActive(false);
        _waveManagerPool.Enqueue(waveManager);
    }

    /// <summary>
    /// Handles changes to the GameManager state and transitions the LevelManager state accordingly.
    /// </summary>
    private async void OnGameManagerStateChanged(GameStateBase newState)
    {
        if (newState is PlayingState)
        {
            if (_currentState.Value is IdleLevelState || _currentState.Value is CompletedLevelState)
            {
                await SetStateAsync<PreparingLevelState>();
            }
        }
        else if (newState is PausedState && _currentState.Value is InProgressLevelState)
        {
            await SetStateAsync<IdleLevelState>();
        }
        else if (newState is MainMenuState)
        {
            await SetStateAsync<IdleLevelState>();
        }
        else if (newState is GameOverState)
        {
            await SetStateAsync<CompletedLevelState>();
        }

        await UniTask.Yield();
    }

    /// <summary>
    /// Calls the `OnUpdate` method of the current state on each frame.
    /// </summary>
    private void Update()
    {
        _currentState.Value?.OnUpdate();
    }
}
