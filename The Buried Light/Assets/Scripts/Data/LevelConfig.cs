using UnityEngine;

[CreateAssetMenu(fileName = "LevelConfig", menuName = "Game/LevelConfig")]
public class LevelConfig : ScriptableObject
{
    [Tooltip("List of phases in this level.")]
    public PhaseConfig[] phases;
}