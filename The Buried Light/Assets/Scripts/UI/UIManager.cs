using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine;
using Zenject;

public class UIManager : MonoBehaviour
{
    public enum UIState
    {
        TitleScreen,
        MainMenu,
        Playing
    }

    private const string MainMenuKey = "MainMenuCanvas";
    private const string GameplayKey = "GameplayCanvas";

    private readonly Dictionary<UIState, string> _uiCanvasKeys = new()
    {
        { UIState.MainMenu, MainMenuKey },
        { UIState.Playing, GameplayKey }
    };

    private UIState _currentState = UIState.TitleScreen; // Default state
    private GameObject _currentCanvas;

    [Inject] private UILoader _uiLoader;
    [Inject] private GameEvents _gameEvents;

    private void Start()
    {
        InitializeSubscriptions();
    }

    private void InitializeSubscriptions()
    {
        _gameEvents.OnMainMenu
            .Subscribe(_ => ChangeUIState(UIState.MainMenu))
            .AddTo(this);

        _gameEvents.OnGameStarted
            .Subscribe(_ => ChangeUIState(UIState.Playing))
            .AddTo(this);
    }

    private async UniTaskVoid ChangeUIState(UIState newState)
    {
        if (_currentState == newState) return;

        // Release the current canvas to free memory
        if (_currentCanvas != null)
        {
            _uiLoader.ReleaseCanvas(_uiCanvasKeys[_currentState]);
            _currentCanvas = null;
        }

        // Update the current state and load the new canvas
        _currentState = newState;
        await LoadCanvasAsync(newState);
    }

    private async UniTask LoadCanvasAsync(UIState state)
    {
        if (_uiCanvasKeys.TryGetValue(state, out var addressableKey))
        {
            _currentCanvas = await _uiLoader.LoadAndInstantiateCanvasAsync(addressableKey);
        }
        else
        {
            Debug.LogError($"No canvas key found for state: {state}");
        }
    }

    private void OnDestroy()
    {
        if (_currentCanvas != null)
        {
            _uiLoader.ReleaseCanvas(_uiCanvasKeys[_currentState]);
        }
    }
}
