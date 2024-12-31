using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class MusicLoader
{
    private AsyncOperationHandle<AudioClip>? _currentHandle;
    private AsyncOperationHandle<AudioClip>? _nextHandle;

    public async void LoadNextTrack(MusicTrack track, System.Action<AudioClip> onLoaded)
    {
        if (_nextHandle.HasValue)
        {
            Addressables.Release(_nextHandle.Value);
        }

        _nextHandle = Addressables.LoadAssetAsync<AudioClip>(track.audioClipReference);
        var clip = await _nextHandle.Value.Task;
        onLoaded?.Invoke(clip);
    }

    public void ReleaseCurrentTrack()
    {
        if (_currentHandle.HasValue)
        {
            Addressables.Release(_currentHandle.Value);
            _currentHandle = null;
        }
    }

    public void SetCurrentTrack(AsyncOperationHandle<AudioClip> handle)
    {
        _currentHandle = handle;
    }
}
