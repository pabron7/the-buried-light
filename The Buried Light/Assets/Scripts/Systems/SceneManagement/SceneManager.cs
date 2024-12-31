using UnityEngine;
using UnityEngine.SceneManagement;
using Cysharp.Threading.Tasks;

public class SceneManager : MonoBehaviour
{
    private string _currentScene = null;

    public async UniTask LoadSceneAsync(string sceneName)
    {
        if (_currentScene == sceneName)
        {
            Debug.LogWarning($"Scene {sceneName} is already loaded.");
            return;
        }

        // Unload the current scene
        if (!string.IsNullOrEmpty(_currentScene))
        {
            await UnloadCurrentSceneAsync();
        }

        // Load the new scene
        Debug.Log($"Loading scene: {sceneName}");
        await UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneName);
        _currentScene = sceneName;

        Debug.Log($"Scene {sceneName} loaded successfully.");
    }

    private async UniTask UnloadCurrentSceneAsync()
    {
        Debug.Log($"Unloading scene: {_currentScene}");
        await UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(_currentScene);
        _currentScene = null;

        Debug.Log("Scene unloaded successfully.");
    }
}
