using UnityEngine;
using Cysharp.Threading.Tasks;
using Zenject;
using UniRx;

public class SaveManager : IInitializable
{
    private readonly ISaveHandler _saveHandler;
    private readonly ILoadHandler _loadHandler;

    [Inject] private PlayerPreferencesStore _playerPreferencesStore;
    [Inject] private PlayerStatsStore _playerStatsStore;
    [Inject] private PlayerDataStore _playerDataStore;
    [Inject] private GameProgressStore _gameProgressStore;

    public PlayerPreferencesStore PlayerPreferencesStore => _playerPreferencesStore;
    public PlayerStatsStore PlayerStatsStore => _playerStatsStore;
    public PlayerDataStore PlayerDataStore => _playerDataStore;
    public GameProgressStore GameProgressStore => _gameProgressStore;

    public ReactiveProperty<bool> IsSaving { get; private set; }
    public ReactiveProperty<bool> IsLoading { get; private set; }

    public SaveManager(ISaveHandler saveHandler, ILoadHandler loadHandler)
    {
        _saveHandler = saveHandler;
        _loadHandler = loadHandler;

        IsSaving = new ReactiveProperty<bool>(false);
        IsLoading = new ReactiveProperty<bool>(false);
    }

    public void Initialize()
    {
        Debug.Log("SaveManager initialized.");
    }

    /// <summary>
    /// Saves all data chunks by converting the reactive properties into serializable objects.
    /// </summary>
    public async UniTask SaveAllAsync()
    {
        IsSaving.Value = true;

        await _saveHandler.SaveAsync("PlayerPreferences", _playerPreferencesStore.ToPlayerPreferences());
        await _saveHandler.SaveAsync("PlayerStats", _playerStatsStore.ToPlayerStats());
        await _saveHandler.SaveAsync("PlayerData", _playerDataStore.ToPlayerData());
        await _saveHandler.SaveAsync("GameProgress", _gameProgressStore.ToGameProgress());

        IsSaving.Value = false;
        Debug.Log("All data saved successfully.");
    }

    /// <summary>
    /// Loads all data chunks and updates their respective stores.
    /// </summary>
    public async UniTask LoadAllAsync()
    {
        IsLoading.Value = true;

        var preferences = await _loadHandler.LoadAsync<PlayerPreferences>("PlayerPreferences");
        var stats = await _loadHandler.LoadAsync<PlayerStats>("PlayerStats");
        var data = await _loadHandler.LoadAsync<PlayerData>("PlayerData");
        var progress = await _loadHandler.LoadAsync<GameProgress>("GameProgress");

        _playerPreferencesStore.UpdatePreferences(preferences);
        _playerStatsStore.UpdateStats(stats);
        _playerDataStore.UpdateData(data);
        _gameProgressStore.UpdateProgress(progress);

        IsLoading.Value = false;
        Debug.Log("All data loaded successfully.");
    }

    /// <summary>
    /// Ensures that all save files exist; if not, creates them with default values.
    /// </summary>
    public async UniTask EnsureFilesExistAsync()
    {
        await EnsureFileExistsAsync("PlayerPreferences", new PlayerPreferences());
        await EnsureFileExistsAsync("PlayerStats", new PlayerStats());
        await EnsureFileExistsAsync("PlayerData", new PlayerData());
        await EnsureFileExistsAsync("GameProgress", new GameProgress());
    }

    /// <summary>
    /// Ensures a specific file exists; creates it with default data if it doesn't.
    /// </summary>
    public async UniTask EnsureFileExistsAsync<T>(string fileName, T defaultData) where T : class
    {
        if (!await _loadHandler.FileExistsAsync(fileName))
        {
            Debug.Log($"File not found: {fileName}. Creating default file.");
            await _saveHandler.SaveAsync(fileName, defaultData);
        }
    }

    /// <summary>
    /// Saves a specific data chunk by converting its reactive properties into a serializable object.
    /// </summary>
    public async UniTask SaveChunkAsync<T>(string fileName, T data) where T : class
    {
        Debug.Log($"Saving data for {fileName}: {JsonUtility.ToJson(data)}");
        IsSaving.Value = true;
        await _saveHandler.SaveAsync(fileName, data);
        IsSaving.Value = false;
        Debug.Log($"{fileName} saved successfully.");
    }

    /// <summary>
    /// Loads a specific data chunk and returns it.
    /// </summary>
    public async UniTask<T> LoadChunkAsync<T>(string fileName) where T : class, new()
    {
        IsLoading.Value = true;
        var data = await _loadHandler.LoadAsync<T>(fileName);
        IsLoading.Value = false;
        Debug.Log($"{fileName} loaded successfully.");
        return data;
    }
}
