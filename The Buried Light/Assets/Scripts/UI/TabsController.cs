using UnityEngine;
using UniRx;
using Cysharp.Threading.Tasks;
using System.Threading;
using System;

public class TabsController : MonoBehaviour
{
    public ReactiveProperty<int> CurrentTab { get; private set; } = new ReactiveProperty<int>(0);
    [SerializeField] private GameObject[] tabs;
    [SerializeField] private float delayDuration = 0.2f;

    private CancellationTokenSource cts;

    private void Start()
    {
        cts = new CancellationTokenSource();

        // Ensure at least one tab is active
        SetActiveTab(0).Forget();
    }

    private void OnDestroy()
    {
        cts?.Cancel();
        cts?.Dispose();
    }

    public async UniTaskVoid SetActiveTab(int tabIndex)
    {
        if (tabIndex < 0 || tabIndex >= tabs.Length)
        {
            Debug.LogWarning($"Invalid tab index: {tabIndex}");
            return;
        }

        CurrentTab.Value = tabIndex;

        try
        {
            // Add a small delay for transitions
            await UniTask.Delay((int)(delayDuration * 1000), cancellationToken: cts.Token);

            // Activate the selected tab and deactivate others
            for (int i = 0; i < tabs.Length; i++)
            {
                if (tabs[i] != null)
                {
                    tabs[i].SetActive(i == tabIndex);
                    Debug.Log($"{tabs[i].name} set to active: {i == tabIndex}");
                }
                else
                {
                    Debug.LogWarning($"Tab at index {i} is null");
                }
            }
        }
        catch (OperationCanceledException)
        {
            Debug.Log("Task was canceled.");
        }
    }
}