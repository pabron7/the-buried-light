using UniRx;

public class PlayerStatsStore
{
    public ReactiveProperty<int> TotalKills { get; private set; } = new ReactiveProperty<int>(0);
    public ReactiveProperty<int> CompletedLevels { get; private set; } = new ReactiveProperty<int>(0);

    /// <summary>
    /// Updates the player stats with new values.
    /// </summary>
    public void UpdateStats(PlayerStats newStats)
    {
        if (newStats == null) return;
        TotalKills.Value = newStats.TotalKills;
        CompletedLevels.Value = newStats.CompletedLevels;
    }

    /// <summary>
    /// Resets the stats to their default values.
    /// </summary>
    public void ResetStats()
    {
        TotalKills.Value = 0;
        CompletedLevels.Value = 0;
    }

    /// <summary>
    /// Returns a serialized PlayerStats object.
    /// </summary>
    public PlayerStats ToPlayerStats()
    {
        return new PlayerStats
        {
            TotalKills = TotalKills.Value,
            CompletedLevels = CompletedLevels.Value
        };
    }
}
