using UnityEngine;
using Cysharp.Threading.Tasks;

public class SceneManager : MonoBehaviour
{
    public async UniTask LoadSceneAsync(string sceneName)
    {
        Debug.Log($"Switching to scene: {sceneName}");

        // Load the new scene in Single mode, automatically unloading the current scene
        var loadOperation = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneName, UnityEngine.SceneManagement.LoadSceneMode.Single);
        await loadOperation;

        Debug.Log($"Scene {sceneName} loaded successfully.");
    }
}
