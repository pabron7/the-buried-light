using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class TabsSwitchButton : MonoBehaviour
{
    [SerializeField] private int tabIndex; // Index of the tab to open
    private TabsController tabsController;

    [Inject]
    public void Construct(TabsController tabsController)
    {
        this.tabsController = tabsController;
    }

    private void Start()
    {
        // Add the button click event dynamically
        Button button = GetComponent<Button>();
        if (button == null)
        {
            Debug.LogError("TabsSwitchButton: No Button component found!");
            return;
        }

        button.onClick.AddListener(() =>
        {
            Debug.Log($"TabsSwitchButton: Button clicked to open tab {tabIndex}.");
            tabsController.OpenTab(tabIndex);
        });
    }
}
