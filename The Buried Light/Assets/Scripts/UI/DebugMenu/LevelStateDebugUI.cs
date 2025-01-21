using UnityEngine;
using TMPro;
using Zenject;
using UniRx;

public class LevelStateDebugUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI stateText;

    [Inject] private LevelManager _levelManager;

    private void Start()
    {
        // Bind the CurrentState and update the UI
        Observable.EveryUpdate()
            .Subscribe(_ =>
            {
                var currentState = _levelManager.CurrentState;
                stateText.text = currentState != null ? $"State: {currentState.GetType().Name}" : "State: None";
            })
            .AddTo(this);
    }
}

