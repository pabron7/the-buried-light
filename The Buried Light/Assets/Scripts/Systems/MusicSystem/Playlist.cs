using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Game/Music/Playlist")]
public class Playlist : ScriptableObject
{
    public string playlistName;
    public List<MusicTrack> tracks = new List<MusicTrack>();
}
