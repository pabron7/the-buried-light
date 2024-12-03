using Zenject;
using UnityEngine;

public class GameInstaller : MonoInstaller
{
    [SerializeField] private EnemyPrefabMapping[] enemyPrefabMappings;
    [SerializeField] private WaveConfig[] waveConfigs;

    public override void InstallBindings()
    {
        // Systems
        Container.Bind<GameManager>().AsSingle();
        Container.BindInterfacesAndSelfTo<InputManager>().AsSingle();
        Container.Bind<InputTestHarness>().FromNewComponentOnNewGameObject().AsSingle();
        Container.Bind<GameFrame>().FromComponentInHierarchy().AsSingle();

        // Level Systems
        Container.Bind<WaveConfig[]>().FromInstance(waveConfigs).AsSingle();
        Container.Bind<WaveManager>().FromComponentInHierarchy().AsSingle();
        Container.Bind<EnemySpawner>().FromComponentInHierarchy().AsSingle();

        // Enemy Systems
        Container.Bind<EnemyPrefabMapping[]>().FromInstance(enemyPrefabMappings).AsSingle();
        Container.Bind<EnemyFactory>().AsSingle();
    }
}
