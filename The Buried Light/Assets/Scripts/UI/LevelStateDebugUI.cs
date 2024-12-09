using UnityEngine;
using TMPro;
using Zenject;
using UniRx;

public class LevelStateDebugUI : MonoBehaviour
{
    [Inject] private LevelManager _levelManager;

    [SerializeField] private TextMeshProUGUI stateText;

    private void Start()
    {
        _levelManager.CurrentState
            .Subscribe(state =>
            {
                if (state != null)
                {
                    stateText.text = $"Level State: {state.GetType().Name}";
                }
                else
                {
                    stateText.text = "Level State: None";
                }
            })
            .AddTo(this);
    }
}
