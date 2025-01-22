using UnityEngine;
using UnityEngine.UI;
using Zenject;

/// <summary>
/// A button that opens a specific tab in the TabsController when clicked.
/// Make sure the GameObject's name matches the declared tabName in the Inspector.
/// </summary>
public class MainMenuTabsSwitchButton : MonoBehaviour
{
    [SerializeField] private string tabName; // The name of the tab to open when the button is clicked
    private TabsController tabsController;

    /// <summary>
    /// Injects the TabsController dependency into this component.
    /// </summary>
    /// <param name="tabsController">The TabsController instance that manages tab switching.</param>
    [Inject]
    public void Construct(TabsController tabsController)
    {
        this.tabsController = tabsController;
    }

    private void Start()
    {
        // Get the Button component on this GameObject
        Button button = GetComponent<Button>();
        if (button == null)
        {
            Debug.LogError("MainMenuTabsSwitchButton: No Button component found!");
            return;
        }

        // Add a listener to open the specified tab when the button is clicked
        button.onClick.AddListener(() =>
        {
            tabsController.OpenTab(tabName);
        });
    }
}
