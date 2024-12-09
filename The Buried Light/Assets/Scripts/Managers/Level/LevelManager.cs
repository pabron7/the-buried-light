using System.Collections.Generic;
using UnityEngine;
using Zenject;
using UniRx;

public class LevelManager : MonoBehaviour
{
    // Reactive property for state management
    private ReactiveProperty<LevelStateBase> _currentState = new ReactiveProperty<LevelStateBase>();

    [SerializeField] private LevelConfig currentLevelConfig;
    private readonly List<WaveManager> _waveManagers = new List<WaveManager>();

    public IReadOnlyReactiveProperty<LevelStateBase> CurrentState => _currentState;

    [Inject] private WaveManager.Factory _waveManagerFactory;

    private void Start()
    {
        SetState<IdleLevelState>(); // Initial state
    }

    /// <summary>
    /// Sets the current state of the LevelManager to a new state.
    /// </summary>
    public void SetState<T>() where T : LevelStateBase
    {
        if (_currentState.Value is T)
        {
            Debug.LogWarning($"LevelManager is already in {typeof(T).Name} state.");
            return;
        }

        // Exit the current state
        _currentState.Value?.OnStateExit();

        // Enter the new state
        var newState = GetComponent<T>() ?? gameObject.AddComponent<T>();
        newState.OnStateEnter(this);

        _currentState.Value = newState; // Update the reactive property
        Debug.Log($"LevelManager state changed to: {typeof(T).Name}");
    }

    /// <summary>
    /// Initializes waves based on the current level configuration.
    /// </summary>
    public void InitializeWaves()
    {
        Debug.Log("Initializing waves...");
        foreach (var waveConfig in currentLevelConfig.waves)
        {
            var waveManager = _waveManagerFactory.Create(waveConfig);
            waveManager.OnWaveComplete += CheckLevelProgress;
            _waveManagers.Add(waveManager);
        }
        Debug.Log($"Initialized {_waveManagers.Count} wave managers.");
    }

    /// <summary>
    /// Checks the progress of all waves and transitions to the completed state if all waves are finished.
    /// </summary>
    private void CheckLevelProgress()
    {
        if (_waveManagers.TrueForAll(w => w.CurrentState == WaveManager.WaveState.Idle))
        {
            Debug.Log("All waves completed. Transitioning to CompletedLevelState.");
            SetState<CompletedLevelState>();
        }
    }

    private void Update()
    {
        // Allow the current state to handle frame-specific logic
        _currentState.Value?.OnUpdate();
    }
}
