[System.Serializable]
public class PlayerPreferences
{
    public float MusicVolume { get; set; } = 1.0f;
    public float SoundVolume { get; set; } = 1.0f;
    public bool ShowTutorial { get; set; } = true;

    public PlayerPreferences() { }
}
