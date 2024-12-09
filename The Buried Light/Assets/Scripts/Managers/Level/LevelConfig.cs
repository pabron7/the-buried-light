using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewLevelConfig", menuName = "Game/Level Config")]
public class LevelConfig : ScriptableObject
{
    [Header("Level Properties")]
    public string levelName;
    public List<WaveConfig> waves; 
    public float levelPreparationDelay = 2f; 
}
