using UnityEngine;

[CreateAssetMenu(fileName = "LevelData_", menuName = "Game/LevelData")]
public class LevelData : ScriptableObject
{
    [Tooltip("List of phases in this level.")]
    public PhaseConfig[] phases;
}