using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TraitCard : MonoBehaviour
{
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text descriptionText;
    [SerializeField] private Image icon;
    [SerializeField] private Button upgradeButton;

    private TraitSO trait;

    /// <summary>
    /// Initializes the Trait Card UI with data from a TraitSO.
    /// </summary>
    public void Initialize(TraitSO traitSO)
    {
        trait = traitSO;

        nameText.text = trait.TraitName;
        descriptionText.text = $"Level: {trait.State}, Effects:\n{GetModifierText()}";
        icon.sprite = trait.TraitIcon;

        upgradeButton.onClick.AddListener(UpgradeTrait);
        UpdateUpgradeButton();
    }

    /// <summary>
    /// Handles upgrading the trait when the button is clicked.
    /// </summary>
    private void UpgradeTrait()
    {
        trait.UpgradeTrait();
        descriptionText.text = $"Level: {trait.State}, Effects:\n{GetModifierText()}";
        UpdateUpgradeButton();
    }

    /// <summary>
    /// Disables the upgrade button when max level is reached.
    /// </summary>
    private void UpdateUpgradeButton()
    {
        upgradeButton.interactable = trait.State < TraitState.Level3;
    }

    /// <summary>
    /// Formats the modifier values for UI display.
    /// </summary>
    private string GetModifierText()
    {
        var modifiers = trait.GetCurrentModifiers();
        if (modifiers.Count == 0) return "No effects";

        string result = "";
        foreach (var mod in modifiers)
        {
            result += $"{mod.ModifierType}: +{mod.ModifierAmount}\n";
        }
        return result;
    }
}
