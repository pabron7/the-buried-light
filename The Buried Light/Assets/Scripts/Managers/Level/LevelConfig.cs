using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelConfig", menuName = "Game/LevelConfig")]
public class LevelConfig : ScriptableObject
{
    [System.Serializable]
    public class Phase
    {
        public string phaseName;
        public List<WaveConfig> waveConfigs;
    }

    public List<Phase> phases;
}
