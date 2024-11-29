public class GameManager
{
    public enum GameState { Playing, Paused, GameOver }
    private GameState _currentState;

    public GameState CurrentState => _currentState;

    public void SetState(GameState newState)
    {
        _currentState = newState;
    }
}
