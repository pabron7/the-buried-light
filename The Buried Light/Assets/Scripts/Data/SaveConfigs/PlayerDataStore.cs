using UniRx;

public class PlayerDataStore
{
    public ReactiveProperty<string> Name { get; private set; } = new ReactiveProperty<string>(string.Empty);
    public ReactiveProperty<string> Email { get; private set; } = new ReactiveProperty<string>(string.Empty);

    /// <summary>
    /// Updates the player data with new values.
    /// </summary>
    public void UpdateData(PlayerData newData)
    {
        if (newData == null) return;
        Name.Value = newData.PlayerName;
        Email.Value = newData.PlayerEmail;
    }

    /// <summary>
    /// Resets the player data to defaults.
    /// </summary>
    public void ResetData()
    {
        Name.Value = string.Empty;
        Email.Value = string.Empty;
    }

    /// <summary>
    /// Returns a serialized PlayerData object.
    /// </summary>
    public PlayerData ToPlayerData()
    {
        return new PlayerData
        {
            PlayerName = Name.Value,
            PlayerEmail = Email.Value
        };
    }
}
