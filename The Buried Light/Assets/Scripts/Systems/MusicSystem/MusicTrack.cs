using UnityEngine;
using UnityEngine.AddressableAssets;

[CreateAssetMenu(menuName = "Game/Music/Track")]
public class MusicTrack : ScriptableObject
{
    public string trackName;
    public AssetReferenceT<AudioClip> audioClipReference;
    public string artist;
    public string album;
    public float volume = 1.0f; // Default volume
}
