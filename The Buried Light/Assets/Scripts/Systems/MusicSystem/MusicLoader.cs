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

        try
        {
            _nextHandle = Addressables.LoadAssetAsync<AudioClip>(track.audioClipReference);
            var clip = await _nextHandle.Value.Task;

            if (clip == null)
            {
                Debug.LogError($"Failed to load AudioClip from Addressable: {track.audioClipReference}");
            }

            onLoaded?.Invoke(clip);
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Error loading AudioClip from Addressable: {track.audioClipReference}. Exception: {ex.Message}");
            onLoaded?.Invoke(null);
        }
    }

    public void ReleaseCurrentTrack()
    {
        if (_currentHandle.HasValue)
        {
            try
            {
                Addressables.Release(_currentHandle.Value);
                _currentHandle = null;
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"Error releasing current track: {ex.Message}");
            }
        }
    }

    public void SetCurrentTrack(AsyncOperationHandle<AudioClip> handle)
    {
        _currentHandle = handle;
    }
}

