using System.Collections.Generic;
using UnityEngine;
using UniRx;
using Zenject;

public class UIManager : MonoBehaviour
{
    public enum UIState
    {
        TitleScreen,
        MainMenu,
        Playing,
        Pause,
        GameOver
    }

    [SerializeField] private GameObject titleScreenCanvas;
    [SerializeField] private GameObject mainMenuCanvas;
    [SerializeField] private GameObject gameplayCanvas;
    [SerializeField] private GameObject pauseCanvas;
    [SerializeField] private GameObject gameOverCanvas;

    private readonly Dictionary<UIState, GameObject> _uiCanvases = new();
    private UIState _currentState = UIState.TitleScreen; // Default state

    [Inject] private GameEvents _gameEvents;

    private void Awake()
    {
        _uiCanvases[UIState.TitleScreen] = titleScreenCanvas;
        _uiCanvases[UIState.MainMenu] = mainMenuCanvas;
        _uiCanvases[UIState.Playing] = gameplayCanvas;
        _uiCanvases[UIState.Pause] = pauseCanvas;
        _uiCanvases[UIState.GameOver] = gameOverCanvas;

        foreach (var canvas in _uiCanvases.Values)
        {
            if (canvas != null) canvas.SetActive(false);
        }

        // Make sure TitleScreen is active at start
        if (titleScreenCanvas != null)
        {
            titleScreenCanvas.SetActive(true);
        }
    }

    private void Start()
    {
        _gameEvents.OnTitleScreen.Subscribe(_ => SetUIState(UIState.TitleScreen)).AddTo(this);
        _gameEvents.OnMainMenu.Subscribe(_ => SetUIState(UIState.MainMenu)).AddTo(this);
        _gameEvents.OnGameStarted.Subscribe(_ => SetUIState(UIState.Playing)).AddTo(this);
        _gameEvents.OnPaused.Subscribe(_ => SetUIState(UIState.Pause)).AddTo(this);
        _gameEvents.OnGameOver.Subscribe(_ => SetUIState(UIState.GameOver)).AddTo(this);
    }

    private void SetUIState(UIState newState)
    {
        if (_currentState == newState) return;

        if (_uiCanvases.ContainsKey(_currentState) && _uiCanvases[_currentState] != null)
        {
            _uiCanvases[_currentState].SetActive(false);
        }

        if (_uiCanvases.ContainsKey(newState) && _uiCanvases[newState] != null)
        {
            _uiCanvases[newState].SetActive(true);
        }

        _currentState = newState;
    }
}