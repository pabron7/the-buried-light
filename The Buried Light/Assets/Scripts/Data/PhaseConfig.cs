using UnityEngine;

[CreateAssetMenu(fileName = "PhaseConfig", menuName = "Game/PhaseConfig")]
public class PhaseConfig : ScriptableObject
{
    [Tooltip("List of waves in this phase.")]
    public WaveConfig[] waves;
}