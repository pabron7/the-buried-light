using Zenject;
using UnityEngine;

public class GameInstaller : MonoInstaller
{
    [SerializeField] private EnemyPrefabMapping[] enemyPrefabMappings;
    [SerializeField] private WaveConfig[] waveConfigs;
    [SerializeField] private ProjectilePoolManager projectilePoolManagerPrefab;
    [SerializeField] private GameObject waveManagerPrefab;

    public override void InstallBindings()
    {
        // Core Systems
        Container.Bind<GameFrame>().FromComponentInHierarchy().AsSingle();
        Container.Bind<ProjectilePoolManager>().FromComponentInNewPrefab(projectilePoolManagerPrefab).AsSingle();
        Container.Bind<PlayerShooting>().FromComponentInHierarchy().AsSingle();

        // Wrapping Utils
        Container.Bind<WrappingUtils>().AsSingle();

        // WaveManager Factory
        Container.BindFactory<WaveManager, WaveManager.Factory>()
            .FromComponentInNewPrefab(waveManagerPrefab)
            .WithGameObjectName("WaveManager")
            .UnderTransformGroup("WaveManagers");
        Container.Bind<WavePoolManager>().AsSingle().NonLazy();
        Container.Bind<PhaseManager>().AsSingle();

        // Bind LevelManager
        Container.Bind<LevelLoader>().AsSingle();
        Container.Bind<LevelManager>().FromComponentInHierarchy().AsSingle();
        Container.Bind<IdleLevelState>().AsTransient();
        Container.Bind<PreparingLevelState>().AsTransient();
        Container.Bind<InProgressLevelState>().AsTransient();
        Container.Bind<CompletedLevelState>().AsTransient();
        Container.Bind<FailedLevelState>().AsTransient();
        Container.Bind<LevelResult>().AsSingle();

        // Enemy Systems
        Container.Bind<EnemyPrefabMapping[]>().FromInstance(enemyPrefabMappings).AsSingle();
        Container.Bind<EnemyFactory>().AsSingle();
        Container.Bind<EnemyPoolManager>().FromComponentInHierarchy().AsSingle();
        Container.Bind<EnemySpawner>().FromComponentInHierarchy().AsSingle();

        // Player Systems
        Container.Bind<PlayerHealth>().FromComponentInHierarchy().AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<PlayerEventsHandler>().AsSingle().NonLazy();
    }
}
