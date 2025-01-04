using UniRx;

public class GameProgressStore
{
    public ReactiveProperty<int> CurrentLevel { get; private set; } = new ReactiveProperty<int>(1);
    public ReactiveProperty<int> CurrentCurrency { get; private set; } = new ReactiveProperty<int>(0);

    /// <summary>
    /// Updates the game progress fields with a new GameProgress object.
    /// </summary>
    public void UpdateProgress(GameProgress newProgress)
    {
        if (newProgress == null) return;

        CurrentLevel.Value = newProgress.CurrentLevel;
        CurrentCurrency.Value = newProgress.CurrentCurrency;
    }

    /// <summary>
    /// Creates a GameProgress object using the current values of the store.
    /// </summary>
    /// <returns>A new GameProgress object populated with current values.</returns>
    public GameProgress ToGameProgress()
    {
        return new GameProgress
        {
            CurrentLevel = CurrentLevel.Value,
            CurrentCurrency = CurrentCurrency.Value
        };
    }

    /// <summary>
    /// Resets the game progress fields to their default values.
    /// </summary>
    public void ResetProgress()
    {
        CurrentLevel.Value = 1;
        CurrentCurrency.Value = 0;
    }
}
