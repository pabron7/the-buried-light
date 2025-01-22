using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages tabs in the main menu, allowing switching between tabs by name.
/// </summary>
public class TabsController : MonoBehaviour
{
    [SerializeField] private List<MainMenuTab> tabs; // List of tabs assigned in the Inspector

    private readonly Dictionary<string, MainMenuTab> tabMapping = new Dictionary<string, MainMenuTab>();
    private MainMenuTab currentTab;

    /// <summary>
    /// Event triggered when the active tab changes. Provides the name of the newly opened tab.
    /// </summary>
    public event Action<string> OnTabChanged;

    private void Start()
    {
        InitializeTabs();
    }

    /// <summary>
    /// Initializes the tabs by mapping their names to the tab objects and hiding all tabs.
    /// </summary>
    private void InitializeTabs()
    {
        if (tabs == null || tabs.Count == 0)
        {
            Debug.LogError("TabsController: No tabs assigned in the Inspector!");
            return;
        }

        foreach (var tab in tabs)
        {
            if (tab != null)
            {
                var tabName = tab.gameObject.name;
                if (!tabMapping.ContainsKey(tabName))
                {
                    tabMapping[tabName] = tab;
                    tab.Hide();
                }
                else
                {
                    Debug.LogWarning($"TabsController: Duplicate tab name '{tabName}' detected!");
                }
            }
        }
    }

    /// <summary>
    /// Opens the tab with the specified name. If the same tab is open, it will be closed. 
    /// Passing an empty string closes all tabs.
    /// </summary>
    /// <param name="tabName">The name of the tab to open. Use an empty string to close all tabs.</param>
    public void OpenTab(string tabName)
    {
        if (string.IsNullOrEmpty(tabName))
        {
            CloseAllTabs();
            OnTabChanged?.Invoke("");
            return;
        }

        if (!tabMapping.TryGetValue(tabName, out var tab))
        {
            Debug.LogWarning($"TabsController: Tab with name '{tabName}' not found.");
            return;
        }

        // If the same tab is clicked again, close all tabs
        if (currentTab == tab)
        {
            CloseAllTabs();
            OnTabChanged?.Invoke("");
            return;
        }

        // Switch to the new tab
        currentTab?.Hide();
        currentTab = tab;
        currentTab.Show();

        OnTabChanged?.Invoke(tabName);
    }

    /// <summary>
    /// Closes all currently open tabs and sets the active tab to null.
    /// </summary>
    public void CloseAllTabs()
    {
        currentTab?.Hide();
        currentTab = null;
    }
}
