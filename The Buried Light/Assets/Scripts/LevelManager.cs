

public class LevelManager
{
    private int _currentLevel = 1;

    public int CurrentLevel => _currentLevel;

    public void AdvanceLevel()
    {
        _currentLevel++;
    }
}

