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

    }

    private async UniTaskVoid ChangeUIState(UIState newState)
    {

    }

    private async UniTask LoadCanvasAsync(UIState state)
    {

    }

    private void OnDestroy()
    {
        if (_currentCanvas != null)
        {
            _uiLoader.ReleaseCanvas(_uiCanvasKeys[_currentState]);
        }
    }
}
