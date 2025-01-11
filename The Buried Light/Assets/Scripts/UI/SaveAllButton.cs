using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class SaveAllButton : MonoBehaviour
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
            Debug.LogError("SaveAllButton requires a Button component.");
        }
    }

    private async void OnButtonPress()
    {
        Debug.Log("Saving all data...");
        await _saveManager.SaveAllAsync();
        Debug.Log("Save completed.");
    }

    private void OnDestroy()
    {
        if (_button != null)
        {
            _button.onClick.RemoveListener(OnButtonPress);
        }
    }
}
