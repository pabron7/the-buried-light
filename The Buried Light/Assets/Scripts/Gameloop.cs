using Zenject;

public class GameLoop : ITickable
{
    private readonly GameManager _gameManager;
    private readonly LevelManager _levelManager;
    private readonly InputManager _inputManager;

    public GameLoop(GameManager gameManager, LevelManager levelManager, InputManager inputManager)
    {
        _gameManager = gameManager;
        _levelManager = levelManager;
        _inputManager = inputManager;
    }

    public void Tick()
    {
        if (_gameManager.CurrentState == GameManager.GameState.Playing)
        {
            _inputManager.Tick();
        }
    }
}

