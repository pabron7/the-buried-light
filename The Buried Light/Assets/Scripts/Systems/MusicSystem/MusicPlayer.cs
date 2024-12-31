using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    private AudioSource _audioSource;

    private void Awake()
    {
        _audioSource = gameObject.AddComponent<AudioSource>();
    }

    public void Play(AudioClip clip, float volume)
    {
        _audioSource.clip = clip;
        _audioSource.volume = volume;
        _audioSource.Play();
    }

    public void Stop()
    {
        if (_audioSource.isPlaying)
        {
            _audioSource.Stop();
        }
    }

    public bool IsPlaying => _audioSource.isPlaying;
}
