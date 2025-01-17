using TMPro;
using UniRx;
using UnityEngine;
using Zenject;

public class CurrencyIndicator : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI currencyText;

    private GameProgressStore _gameProgressStore;

    [Inject]
    public void Construct(GameProgressStore gameProgressStore)
    {
        _gameProgressStore = gameProgressStore;
    }

    private void Start()
    {
        // Subscribe to changes in the CurrentLevel ReactiveProperty
        _gameProgressStore.CurrentCurrency
            .Subscribe(UpdateLevelText)
            .AddTo(this);
    }

    /// <summary>
    /// Updates the level text in the UI.
    /// </summary>
    /// <param name="val">The current level value.</param>
    private void UpdateLevelText(int val)
    {
        currencyText.text = $"Currency: {val}";
    }
}