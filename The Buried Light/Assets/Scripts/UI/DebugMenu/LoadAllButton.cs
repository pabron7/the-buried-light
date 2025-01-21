using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class LoadAllButton : MonoBehaviour
{
    private SaveManager _saveManager;
    private Button _button;

    [Inject]
    public void Construct(SaveManager saveManager)
    {
        _saveManager = saveManager;
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
            Debug.LogError("LoadAllButton requires a Button component.");
        }
    }

    private async void OnButtonPress()
    {
        Debug.Log("Loading all data...");
        await _saveManager.LoadAllAsync();
        Debug.Log("Load completed.");
    }

    private void OnDestroy()
    {
        if (_button != null)
        {
            _button.onClick.RemoveListener(OnButtonPress);
        }
    }
}
