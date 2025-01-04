using Zenject;
using UnityEngine;

public class SaveInitializer : MonoBehaviour
{
    [Inject] private SaveManager _saveManager;

    private async void Start()
    {
        await _saveManager.EnsureFileExistsAsync("PlayerPreferences", new PlayerPreferences());
        await _saveManager.EnsureFileExistsAsync("PlayerStats", new PlayerStats());
        await _saveManager.EnsureFileExistsAsync("PlayerData", new PlayerData());
        await _saveManager.EnsureFileExistsAsync("GameProgress", new GameProgress());

        Debug.Log("All save files initialized.");
    }
}
