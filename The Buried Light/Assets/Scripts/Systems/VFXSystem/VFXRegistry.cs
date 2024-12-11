using System.Collections.Generic;
using UnityEngine;

public class VFXRegistry : MonoBehaviour
{
    [SerializeField] private List<VFXEffect> vfxEffects;

    private Dictionary<string, VFXEffect> _vfxEffectMap;

    private void Awake()
    {
        _vfxEffectMap = new Dictionary<string, VFXEffect>();

        foreach (var vfxEffect in vfxEffects)
        {
            if (!_vfxEffectMap.ContainsKey(vfxEffect.Id))
            {
                _vfxEffectMap[vfxEffect.Id] = vfxEffect;
            }
            else
            {
                Debug.LogWarning($"Duplicate VFX ID: {vfxEffect.Id}");
            }
        }
    }

    public VFXEffect GetVFXEffect(string id)
    {
        if (_vfxEffectMap.TryGetValue(id, out var vfxEffect))
        {
            return vfxEffect;
        }

        Debug.LogWarning($"VFXRegistry: VFX effect with ID '{id}' not found.");
        return null;
    }
}