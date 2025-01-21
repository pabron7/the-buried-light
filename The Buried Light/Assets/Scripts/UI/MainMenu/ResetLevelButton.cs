using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class SetLevelToOneButton : MonoBehaviour
{
    private GameProgressStore _gameProgressStore;
    private Button _button;

    [Inject]
    public void Construct(GameProgressStore gameProgressStore)
    {
        _gameProgressStore = gameProgressStore;
    }

    private void Awake()
    {
        _button = GetComponent<Button>();
        if (_button != null)
        {
            _button.onClick.AddListener(OnButtonPress);
        }
        else
        {
            Debug.LogError("SetLevelToOneButton requires a Button component.");
        }
    }

    private async void OnButtonPress()
    {
        Debug.Log("Setting level to 1...");
        _gameProgressStore.CurrentLevel.Value = 1;
    }

    private void OnDestroy()
    {
        if (_button != null)
        {
            _button.onClick.RemoveListener(OnButtonPress);
        }
    }
}
