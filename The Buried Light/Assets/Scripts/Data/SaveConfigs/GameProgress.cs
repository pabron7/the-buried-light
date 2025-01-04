[System.Serializable]
public class GameProgress
{
    public int CurrentLevel { get; set; } = 1;
    public int CurrentCurrency { get; set; }  = 0;

    public GameProgress() { }
}