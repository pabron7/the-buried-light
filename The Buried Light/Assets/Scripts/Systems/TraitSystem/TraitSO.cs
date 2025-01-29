using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewTrait", menuName = "Game/Trait", order = 0)]
public class TraitSO : ScriptableObject
{
    public string TraitName;
    public string TraitDescription;
    public Sprite TraitIcon;

    [Tooltip("List of level-specific modifiers. First entry is Level 1, second is Level 2, and so on.")]
    public List<List<TraitModifier>> LevelModifiers = new();

    public TraitState State = TraitState.Locked; 

    /// <summary>
    /// Gets the modifiers for the current level.
    /// </summary>
    public List<TraitModifier> GetCurrentModifiers()
    {
        int levelIndex = (int)State - 1;
        if (levelIndex >= 0 && levelIndex < LevelModifiers.Count)
        {
            return LevelModifiers[levelIndex];
        }
        return new List<TraitModifier>(); // Return empty list if locked
    }

    /// <summary>
    /// Upgrades the trait to the next level.
    /// </summary>
    public void UpgradeTrait()
    {
        if (State < TraitState.Level3)
        {
            State++;
        }
    }
}
