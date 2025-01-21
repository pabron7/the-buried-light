using UnityEngine;
using TMPro;
using UniRx;
using Zenject;

public class GameStateDebugUI : MonoBehaviour
{
    [Inject] private GameManager _gameManager;

    [SerializeField] private TextMeshProUGUI stateText;

    private void Start()
    {
        _gameManager.CurrentState
            .Subscribe(state =>
            {
                if (state != null)
                {
                    stateText.text = $"Game State: {state.GetType().Name}";
                }
                else
                {
                    stateText.text = "State: None";
                }
            })
            .AddTo(this);
    }
}
