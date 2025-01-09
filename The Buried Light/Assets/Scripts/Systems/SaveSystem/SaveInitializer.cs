using Zenject;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class SaveInitializer : MonoBehaviour
{
    [Inject] private SaveManager _saveManager;

    private async void Start()
    {
        // Ensure save files exist with default data if not already present
        await _saveManager.EnsureFileExistsAsync("PlayerPreferences", new PlayerPreferences());
        await _saveManager.EnsureFileExistsAsync("PlayerStats", new PlayerStats());
        await _saveManager.EnsureFileExistsAsync("PlayerData", new PlayerData());
        await _saveManager.EnsureFileExistsAsync("GameProgress", new GameProgress());

        // Load all saved data and update the stores
        await _saveManager.LoadAllAsync();
    }
}
