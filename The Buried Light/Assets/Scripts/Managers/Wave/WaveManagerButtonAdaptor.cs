using UnityEngine;
using Zenject;

public class WaveManagerButtonAdapter : MonoBehaviour
{
    private WaveManager _waveManager;

    [Inject]
    public void Construct(WaveManager waveManager)
    {
        _waveManager = waveManager;
    }


    public void StartNextWave()
    {
        _waveManager.StartNextWave();
    }
}
