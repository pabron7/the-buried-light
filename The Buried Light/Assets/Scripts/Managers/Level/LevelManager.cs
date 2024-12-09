using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class LevelManager : MonoBehaviour
{
    private LevelStateBase _currentState;

    [SerializeField] private LevelConfig currentLevelConfig;
    private List<WaveManager> _waveManagers = new List<WaveManager>();

    public LevelStateBase CurrentState => _currentState;

    [Inject] private WaveManager.Factory _waveManagerFactory;

    private void Start()
    {
        SetState<IdleLevelState>(); // Initial state
    }

    public void SetState<T>() where T : LevelStateBase
    {
        if (_currentState is T)
        {
            Debug.LogWarning($"LevelManager is already in {typeof(T).Name} state.");
            return;
        }

        _currentState?.OnStateExit();
        _currentState = GetComponent<T>() ?? gameObject.AddComponent<T>();
        _currentState.OnStateEnter(this);
        Debug.Log($"LevelManager state changed to: {typeof(T).Name}");
    }

    public void InitializeWaves()
    {
        foreach (var waveConfig in currentLevelConfig.waves)
        {
            var waveManager = _waveManagerFactory.Create(waveConfig);
            waveManager.OnWaveComplete += CheckLevelProgress;
            _waveManagers.Add(waveManager);
        }
    }

    private void CheckLevelProgress()
    {
        if (_waveManagers.TrueForAll(w => w.CurrentState == WaveManager.WaveState.Idle))
        {
            SetState<CompletedLevelState>();
        }
    }
}
