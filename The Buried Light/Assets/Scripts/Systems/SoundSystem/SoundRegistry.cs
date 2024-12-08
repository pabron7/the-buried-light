using System.Collections.Generic;
using UnityEngine;

public class SoundRegistry : MonoBehaviour
{
    [SerializeField] private List<SoundEffect> soundEffects; 

    private Dictionary<string, SoundEffect> _soundEffectMap;

    private void Awake()
    {
        _soundEffectMap = new Dictionary<string, SoundEffect>();

        foreach (var soundEffect in soundEffects)
        {
            if (!_soundEffectMap.ContainsKey(soundEffect.Id))
            {
                _soundEffectMap[soundEffect.Id] = soundEffect;
            }
            else
            {
                Debug.LogWarning($"Duplicate sound ID: {soundEffect.Id}");
            }
        }
    }

    public SoundEffect GetSoundEffect(string id)
    {
        if (_soundEffectMap.TryGetValue(id, out var soundEffect))
        {
            return soundEffect;
        }

        Debug.LogWarning($"SoundRegistry: Sound effect with ID '{id}' not found.");
        return null;
    }

}

