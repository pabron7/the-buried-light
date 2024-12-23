using Zenject;
using UnityEngine;

public class GameInstaller : MonoInstaller
{
    [SerializeField] private EnemyPrefabMapping[] enemyPrefabMappings;
    [SerializeField] private WaveConfig[] waveConfigs;
    [SerializeField] private ProjectilePoolManager projectilePoolManagerPrefab;
    [SerializeField] private GameObject waveManagerPrefab;
    [SerializeField] private LevelConfig levelConfig;

    [SerializeField] private SoundManager soundManagerPrefab;
    [SerializeField] private SoundRegistry soundRegistry;
    [SerializeField] private VFXRegistry vFXRegistry;

    public override void InstallBindings()
    {
        // Game Manager
        Container.Bind<GameManager>().FromComponentInHierarchy().AsSingle().NonLazy();

        // Wrapping Utils
        Container.Bind<WrappingUtils>().AsSingle();

        // Input Systems
        Container.BindInterfacesAndSelfTo<InputManager>().AsSingle();

        // Core Systems
        Container.Bind<GameFrame>().FromComponentInHierarchy().AsSingle();
        Container.Bind<ProjectilePoolManager>().FromComponentInNewPrefab(projectilePoolManagerPrefab).AsSingle();
        Container.Bind<PlayerShooting>().FromComponentInHierarchy().AsSingle();

        // Event System
        Container.Bind<EventManager>().AsSingle().NonLazy();

        // Sound Systems
        Container.Bind<SoundRegistry>().FromInstance(soundRegistry).AsSingle();
        Container.Bind<SoundManager>().FromComponentInHierarchy().AsSingle();

        Container.Bind<VFXManager>().FromComponentInHierarchy().AsSingle();
        Container.Bind<VFXRegistry>().FromInstance(vFXRegistry).AsSingle();

        // WaveManager Factory
        Container.BindFactory<WaveManager, WaveManager.Factory>()
            .FromComponentInNewPrefab(waveManagerPrefab)
            .WithGameObjectName("WaveManager")
            .UnderTransformGroup("WaveManagers");
        Container.Bind<WavePoolManager>().AsSingle().NonLazy();
        Container.Bind<PhaseManager>().AsSingle();
        Container.Bind<LevelConfig>().FromInstance(levelConfig).AsSingle();

        // Bind LevelManager
        Container.Bind<LevelManager>().FromComponentInHierarchy().AsSingle();

        // Enemy Systems
        Container.Bind<EnemyPrefabMapping[]>().FromInstance(enemyPrefabMappings).AsSingle();
        Container.Bind<EnemyFactory>().AsSingle();
        Container.Bind<EnemyPoolManager>().FromComponentInHierarchy().AsSingle();
        Container.Bind<EnemySpawner>().FromComponentInHierarchy().AsSingle();

        // Specific event publishers
        Container.Bind<EnemyEvents>().AsSingle();
        Container.Bind<PlayerEvents>().AsSingle();
        Container.Bind<GameEvents>().AsSingle();

        // Player Systems
        Container.Bind<IHealth>().To<PlayerHealth>().FromInstance(FindObjectOfType<PlayerHealth>()).AsSingle();

        // UI Systems
        Container.Bind<UIManager>().FromComponentInHierarchy().AsSingle();

        // Score
        Container.Bind<ScoreManager>().AsSingle();

    }
}