using TMPro;
using UniRx;
using UnityEngine;
using Zenject;

public class CurrentLevelUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI levelText;

    private GameProgressStore _gameProgressStore;

    [Inject]
    public void Construct(GameProgressStore gameProgressStore)
    {
        _gameProgressStore = gameProgressStore;
    }

    private void Start()
    {
        // Subscribe to changes in the CurrentLevel ReactiveProperty
        _gameProgressStore.CurrentLevel
            .Subscribe(UpdateLevelText)
            .AddTo(this);
    }

    /// <summary>
    /// Updates the level text in the UI.
    /// </summary>
    /// <param name="level">The current level value.</param>
    private void UpdateLevelText(int level)
    {
        levelText.text = $"Level {level}";
    }
}
