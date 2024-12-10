using UnityEngine;
using UniRx;
using Zenject;

public class SoundManager : MonoBehaviour
{
    private SoundRegistry _soundRegistry;
    private AudioSource[] _audioSources;
    private const int AudioSourceCount = 4;
    private readonly SoundEffectMapper _soundEffectMapper = new();

    [Inject]
    public void Construct(SoundRegistry soundRegistry, EventManager eventManager)
    {
        _soundRegistry = soundRegistry;

        // Subscribe to EventManager's OnCommandExecuted
        eventManager.OnCommandExecuted
            .Subscribe(HandleCommand)
            .AddTo(this);
    }

    private void Awake()
    {
        CreateAudioSources();
    }

    /// <summary>
    /// Creates and configures a pool of audio sources.
    /// </summary>
    private void CreateAudioSources()
    {
        _audioSources = new AudioSource[AudioSourceCount];
        for (int i = 0; i < AudioSourceCount; i++)
        {
            var audioSourceObject = new GameObject($"AudioSource_{i}");
            audioSourceObject.transform.SetParent(transform);
            _audioSources[i] = audioSourceObject.AddComponent<AudioSource>();
        }

        Debug.Log($"Created {AudioSourceCount} AudioSources.");
    }

    /// <summary>
    /// Handles commands by mapping them to sound effects and playing them.
    /// </summary>
    private void HandleCommand(ICommand command)
    {
        var soundId = _soundEffectMapper.GetSoundIdForCommand(command);
        if (soundId != null)
        {
            PlaySound(soundId);
        }
    }

    /// <summary>
    /// Plays the sound effect corresponding to the given sound ID.
    /// </summary>
    private void PlaySound(string soundId)
    {
        var soundEffect = _soundRegistry.GetSoundEffect(soundId);
        if (soundEffect == null)
        {
            Debug.LogWarning($"SoundManager: Sound effect with ID '{soundId}' not found.");
            return;
        }

        var availableSource = GetAvailableAudioSource();
        if (availableSource != null)
        {
            availableSource.pitch = soundEffect.Pitch;
            availableSource.PlayOneShot(soundEffect.Clip, soundEffect.Volume);
        }
        else
        {
            Debug.LogWarning("SoundManager: No available AudioSource to play sound.");
        }
    }

    /// <summary>
    /// Retrieves an available audio source from the pool.
    /// </summary>
    private AudioSource GetAvailableAudioSource()
    {
        foreach (var audioSource in _audioSources)
        {
            if (!audioSource.isPlaying)
            {
                return audioSource;
            }
        }

        return null;
    }
}
