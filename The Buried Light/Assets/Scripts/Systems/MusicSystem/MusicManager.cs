using UnityEngine;
using System.Collections.Generic;
using UniRx;
using Zenject;

public class MusicManager : MonoBehaviour
{
    [SerializeField] private List<Playlist> playlists;
    private Dictionary<string, Playlist> _playlistMap;

    private MusicLoader _musicLoader;
    private MusicPlayer _musicPlayer;

    private Playlist _currentPlaylist;
    private int _currentTrackIndex;

    [Inject]
    public void Construct(GameEvents gameEvents)
    {
        // Subscribe to game events to switch playlists
        gameEvents.OnMainMenu.Subscribe(_ => SwitchPlaylist("MainMenu")).AddTo(this);
        gameEvents.OnGameStarted.Subscribe(_ => SwitchPlaylist("GameplayNormal")).AddTo(this);
        gameEvents.OnPaused.Subscribe(_ => SwitchPlaylist("Paused")).AddTo(this);
        gameEvents.OnGameOver.Subscribe(_ => SwitchPlaylist("GameOver")).AddTo(this);
        gameEvents.OnTitleScreen.Subscribe(_ => SwitchPlaylist("TitleScreen")).AddTo(this);
    }

    private void Awake()
    {
        _musicLoader = new MusicLoader();
        _musicPlayer = gameObject.AddComponent<MusicPlayer>();

        _playlistMap = new Dictionary<string, Playlist>();
        foreach (var playlist in playlists)
        {
            _playlistMap[playlist.playlistName] = playlist;
        }
    }

    private void Start()
    {
        SwitchPlaylist("MainMenu");
    }

    public void SwitchPlaylist(string playlistName)
    {
        if (_playlistMap.TryGetValue(playlistName, out var newPlaylist))
        {
            if (newPlaylist.tracks == null || newPlaylist.tracks.Count == 0)
            {
                Debug.LogError($"Playlist '{playlistName}' has no tracks.");
                return;
            }

            _currentPlaylist = newPlaylist;
            _currentTrackIndex = 0;
            PlayNextTrack();
        }
        else
        {
            Debug.LogError($"Playlist '{playlistName}' not found.");
        }
    }

    private void PlayNextTrack()
    {
        if (_currentPlaylist == null || _currentPlaylist.tracks.Count == 0)
        {
            Debug.LogWarning("No tracks in the current playlist.");
            return;
        }

        var currentTrack = _currentPlaylist.tracks[_currentTrackIndex];

        try
        {
            _musicLoader.ReleaseCurrentTrack();
            _musicLoader.LoadNextTrack(currentTrack, clip =>
            {
                if (clip == null)
                {
                    Debug.LogError($"Failed to load track: {currentTrack.audioClipReference}");
                    return;
                }

                _musicPlayer.Play(clip, currentTrack.volume);

                // Prepare the next track
                _currentTrackIndex = (_currentTrackIndex + 1) % _currentPlaylist.tracks.Count;
                var nextTrack = _currentPlaylist.tracks[_currentTrackIndex];
                _musicLoader.LoadNextTrack(nextTrack, null);
            });
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Error playing next track: {ex.Message}");
        }
    }

    private void Update()
    {
        // Check if the current track has finished
        if (!_musicPlayer.IsPlaying && _currentPlaylist != null)
        {
            PlayNextTrack();
        }
    }
}
