using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Cysharp.Threading.Tasks;
using Zenject;

public class LevelLoader
{
    private LevelConfig _currentLevelConfig;

    [Inject] private GameEvents _gameEvents;

    /// <summary>
    /// Loads a LevelConfig by its addressable address.
    /// </summary>
    public async UniTask<LevelConfig> LoadLevelAsync(string levelAddress)
    {
        if (_currentLevelConfig != null)
        {
            Debug.LogError("LevelLoader: A level is already loaded. Release the current level first.");
            return null;
        }

        var handle = Addressables.LoadAssetAsync<LevelConfig>(levelAddress);
        await handle.Task;

        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            _currentLevelConfig = handle.Result;
            Debug.Log($"LevelLoader: Successfully loaded {levelAddress}");
            _gameEvents.NotifyLevelLoad(levelAddress);
            return _currentLevelConfig;
        }

        Debug.LogError($"LevelLoader: Failed to load level {levelAddress}");
        return null;
    }

    /// <summary>
    /// Releases the currently loaded level.
    /// </summary>
    public void ReleaseLevel()
    {
        if (_currentLevelConfig == null)
        {
            Debug.LogWarning("LevelLoader: No level to release.");
            return;
        }

        Addressables.Release(_currentLevelConfig);
        _gameEvents.NotifyLevelRelease(_currentLevelConfig.name);
        _currentLevelConfig = null;
    }
}
