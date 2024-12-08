using UniRx;
using UnityEngine;
using Zenject;
using System;

public class SoundManager : MonoBehaviour
{
    private SoundRegistry _registry;
    [SerializeField] private AudioSource audioSource;

    [Inject]
    public void Construct(SoundRegistry registry)
    {
        _registry = registry;
    }

    public void Initialize(IObservable<string> soundStream)
    {
        soundStream.Subscribe(PlaySound).AddTo(this);
        Debug.Log("SoundManager Initialized and Subscribed to Sound Stream.");
    }

    private void PlaySound(string soundId)
    {
        var soundEffect = _registry.GetSoundEffect(soundId);
        if (soundEffect == null)
        {
            return;
        }
        audioSource.pitch = soundEffect.Pitch;
        audioSource.PlayOneShot(soundEffect.Clip, soundEffect.Volume);
    }
}