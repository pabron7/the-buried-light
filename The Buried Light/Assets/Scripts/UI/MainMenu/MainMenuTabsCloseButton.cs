using UnityEngine;
using UnityEngine.UI;
using Zenject;

/// <summary>
/// A button that closes any currently open tab in the TabsController when clicked.
/// </summary>
public class MainMenuTabsCloseTabsButton : MonoBehaviour
{
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
            Debug.LogError("MainMenuTabsCloseTabsButton: No Button component found!");
            return;
        }

        // Add a listener to close all tabs when the button is clicked
        button.onClick.AddListener(() =>
        {
            tabsController.CloseAllTabs();
        });
    }
}
