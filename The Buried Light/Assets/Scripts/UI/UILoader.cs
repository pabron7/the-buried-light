using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class UILoader
{
    private readonly Dictionary<string, GameObject> _loadedCanvases = new();

    /// <summary>
    /// Loads and instantiates a canvas as an Addressable asset.
    /// </summary>
    public async UniTask<GameObject> LoadAndInstantiateCanvasAsync(string addressableKey)
    {
        if (_loadedCanvases.TryGetValue(addressableKey, out var canvas))
        {
            return canvas;
        }

        var handle = Addressables.LoadAssetAsync<GameObject>(addressableKey);
        await handle.ToUniTask();

        if (handle.Status == UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationStatus.Succeeded)
        {
            canvas = Object.Instantiate(handle.Result);
            _loadedCanvases[addressableKey] = canvas;
            return canvas;
        }

        Debug.LogError($"Failed to load canvas with key: {addressableKey}");
        return null;
    }

    /// <summary>
    /// Releases a canvas and removes it from memory.
    /// </summary>
    public void ReleaseCanvas(string addressableKey)
    {
        if (_loadedCanvases.TryGetValue(addressableKey, out var canvas))
        {
            Object.Destroy(canvas);
            Addressables.Release(canvas);
            _loadedCanvases.Remove(addressableKey);
        }
    }

    /// <summary>
    /// Releases all loaded canvases.
    /// </summary>
    public void ReleaseAllCanvases()
    {
        foreach (var canvas in _loadedCanvases.Values)
        {
            Object.Destroy(canvas);
        }

        _loadedCanvases.Clear();
    }
}
