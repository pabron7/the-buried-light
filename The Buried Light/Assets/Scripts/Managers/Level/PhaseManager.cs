using System.Collections.Generic;
using UnityEngine;
using Zenject;
using Cysharp.Threading.Tasks;
using UniRx;
using System;

public class PhaseManager
{
    private LevelData _levelConfig;
    private readonly WavePoolManager _wavePoolManager;

    private readonly CompositeDisposable _disposables = new CompositeDisposable();

    private int _currentPhaseIndex = 0;
    private int _completedWavesCount = 0;
    private int _totalWavesInPhase = 0;
    private int _totalEnemiesInPhase = 0;
    private int _killedEnemiesInPhase = 0;

    private bool _isHalted = false;

    [SerializeField] private float waveStartDelay = 2f;

    [Inject] private GameEvents _gameEvents;
    [Inject] private EnemyEvents _enemyEvents;

    public PhaseManager(WavePoolManager wavePoolManager, GameEvents gameEvents, EnemyEvents enemyEvents)
    {
        _wavePoolManager = wavePoolManager ?? throw new ArgumentNullException(nameof(wavePoolManager));
        _gameEvents = gameEvents ?? throw new ArgumentNullException(nameof(gameEvents));
        _enemyEvents = enemyEvents ?? throw new ArgumentNullException(nameof(enemyEvents));

        // Subscribe to enemy killed events
        _enemyEvents.OnEnemyKilled
            .Subscribe(_ => HandleEnemyKilled())
            .AddTo(_disposables);
    }

    /// <summary> The current phase index. </summary>
    public int CurrentPhaseIndex => _currentPhaseIndex;

    /// <summary> The total number of phases. </summary>
    public int TotalPhases => _levelConfig.phases.Length;

    /// <summary>
    /// Determines if there are more phases to process in the level.
    /// </summary>
    public bool HasMorePhases() => _currentPhaseIndex < TotalPhases;

    /// <summary>
    /// Sets the LevelConfig for this PhaseManager instance.
    /// </summary>
    public void SetLevelConfig(LevelData levelConfig)
    {
        _levelConfig = levelConfig ?? throw new ArgumentNullException(nameof(levelConfig));
        ResetPhases();
    }

    /// <summary>
    /// Starts the next phase asynchronously and manages wave spawning.
    /// </summary>
    public async UniTask StartPhaseAsync()
    {
        if (!HasMorePhases())
        {
            Debug.LogWarning("PhaseManager: No more phases available.");
            return;
        }

        ResetPhaseCounters();

        var currentPhase = _levelConfig.phases[_currentPhaseIndex];
        _currentPhaseIndex++;

        Debug.Log($"PhaseManager: Starting Phase {_currentPhaseIndex}");
        _gameEvents.NotifyPhaseStart(_currentPhaseIndex);

        // Calculate total waves and enemies for the phase
        _totalWavesInPhase = currentPhase.waves.Length;
        foreach (var waveConfig in currentPhase.waves)
        {
            _totalEnemiesInPhase += waveConfig.enemyCount;
        }

        Debug.Log($"PhaseManager: Total enemies in phase {_currentPhaseIndex}: {_totalEnemiesInPhase}");
        Debug.Log($"PhaseManager: Total waves in phase {_currentPhaseIndex}: {_totalWavesInPhase}");

        // Activate waves in the phase with delays
        var activeWaveManagers = new List<WaveManager>();
        foreach (var waveConfig in currentPhase.waves)
        {
            await UniTask.Delay((int)(waveStartDelay * 1000)); // Delay before starting each wave

            var waveManager = _wavePoolManager.GetAvailableWaveManager();
            if (waveManager == null)
            {
                Debug.LogError("PhaseManager: Failed to retrieve WaveManager.");
                continue;
            }

            waveManager.Initialize(waveConfig);
            waveManager.OnWaveComplete += HandleWaveCompletion; // Subscribe to wave completion
            waveManager.gameObject.SetActive(true);
            waveManager.StartWave();
            activeWaveManagers.Add(waveManager);
        }

        // Wait until all waves in the phase are complete and all enemies are killed
        await UniTask.WaitUntil(() =>
            _completedWavesCount >= _totalWavesInPhase &&
            _killedEnemiesInPhase >= _totalEnemiesInPhase
        );

        // Cleanup and notify phase completion
        foreach (var waveManager in activeWaveManagers)
        {
            waveManager.ResetWave();
            waveManager.OnWaveComplete -= HandleWaveCompletion; // Unsubscribe from wave completion
            waveManager.gameObject.SetActive(false);
            _wavePoolManager.ReturnWaveManagerToPool(waveManager);
        }

        Debug.Log("PhaseManager: Phase complete.");
        _gameEvents.NotifyPhaseEnd(_currentPhaseIndex);
    }

    /// <summary>
    /// Resets the phase progression for a new cycle.
    /// </summary>
    public void ResetPhases()
    {
        _currentPhaseIndex = 0;
        Debug.Log("PhaseManager: Phases reset.");
    }

    /// <summary>
    /// Resets all active waves in the phase.
    /// </summary>
    public void ResetWaves()
    {
        Debug.Log("PhaseManager: Resetting all waves.");
        _wavePoolManager.ResetPool();
    }

    /// <summary>
    /// Resets counters for a new phase.
    /// </summary>
    private void ResetPhaseCounters()
    {
        _totalEnemiesInPhase = 0;
        _killedEnemiesInPhase = 0;
        _completedWavesCount = 0;
        _totalWavesInPhase = 0;
    }

    /// <summary>
    /// Handles the completion of a wave.
    /// </summary>
    private void HandleWaveCompletion()
    {
        _completedWavesCount++;
        Debug.Log($"PhaseManager: Wave completed. {_completedWavesCount}/{_totalWavesInPhase} waves completed.");
    }

    /// <summary>
    /// Handles enemy killed events and updates the phase kill count.
    /// </summary>
    private void HandleEnemyKilled()
    {
        _killedEnemiesInPhase++;
        //Debug.Log($"PhaseManager: Enemy killed. {_killedEnemiesInPhase}/{_totalEnemiesInPhase} enemies killed.");
    }

    /// <summary>
    /// Cleans up subscriptions and resets the state.
    /// </summary>
    public void Cleanup()
    {
        _disposables.Clear();
    }

    public void HaltPhase()
    {
        _isHalted = true;
        foreach (var waveManager in _wavePoolManager.GetActiveWaveManagers())
        {
            waveManager.HaltWave();
        }
    }

    public void ContinuePhase()
    {
        _isHalted = false;
        foreach (var waveManager in _wavePoolManager.GetActiveWaveManagers())
        {
            waveManager.ContinueWave();
        }
    }
}
