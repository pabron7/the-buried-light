using UniRx;

public class PlayerPreferencesStore
{
    public ReactiveProperty<float> MusicVolume { get; private set; } = new ReactiveProperty<float>(1.0f);
    public ReactiveProperty<float> SoundVolume { get; private set; } = new ReactiveProperty<float>(1.0f);
    public ReactiveProperty<bool> ShowTutorial { get; private set; } = new ReactiveProperty<bool>(true);

    /// <summary>
    /// Updates the player preferences with new values.
    /// </summary>
    public void UpdatePreferences(PlayerPreferences newPreferences)
    {
        if (newPreferences == null) return;
        MusicVolume.Value = newPreferences.MusicVolume;
        SoundVolume.Value = newPreferences.SoundVolume;
        ShowTutorial.Value = newPreferences.ShowTutorial;
    }

    /// <summary>
    /// Resets the preferences to their default values.
    /// </summary>
    public void ResetPreferences()
    {
        MusicVolume.Value = 1.0f;
        SoundVolume.Value = 1.0f;
        ShowTutorial.Value = true;
    }

    /// <summary>
    /// Returns a serialized PlayerPreferences object.
    /// </summary>
    public PlayerPreferences ToPlayerPreferences()
    {
        return new PlayerPreferences
        {
            MusicVolume = MusicVolume.Value,
            SoundVolume = SoundVolume.Value,
            ShowTutorial = ShowTutorial.Value
        };
    }
}
