using Zenject;
using UnityEngine;

public class GameInstaller : MonoInstaller
{
    [SerializeField] private EnemyPrefabMapping[] enemyPrefabMappings;
    [SerializeField] private WaveConfig[] waveConfigs;
    [SerializeField] private ProjectilePoolManager projectilePoolManagerPrefab;

    [SerializeField] private SoundManager soundManagerPrefab;
    [SerializeField] private SoundRegistry soundRegistry;

    public override void InstallBindings()
    {
        // Systems
        Container.Bind<GameManager>().AsSingle();
        Container.BindInterfacesAndSelfTo<InputManager>().AsSingle();
        Container.Bind<GameFrame>().FromComponentInHierarchy().AsSingle();
        Container.Bind<ProjectilePoolManager>().FromComponentInNewPrefab(projectilePoolManagerPrefab).AsSingle();
        Container.Bind<PlayerShooting>().FromComponentInHierarchy().AsSingle();
        Container.Bind<EventManager>().AsSingle();

        Container.Bind<SoundRegistry>().FromComponentInHierarchy().AsSingle();
        Container.Bind<SoundManager>().FromComponentInHierarchy().AsSingle();

        // Level Systems
        Container.Bind<WaveConfig[]>().FromInstance(waveConfigs).AsSingle();
        Container.Bind<WaveManager>().FromComponentInHierarchy().AsSingle();
        Container.Bind<EnemySpawner>().FromComponentInHierarchy().AsSingle();

        // Enemy Systems
        Container.Bind<EnemyPrefabMapping[]>().FromInstance(enemyPrefabMappings).AsSingle();
        Container.Bind<EnemyFactory>().AsSingle();
        Container.Bind<EnemyPoolManager>().FromComponentInHierarchy().AsSingle();

        //Player
        Container.Bind<IHealth>().To<PlayerHealth>().FromInstance(FindObjectOfType<PlayerHealth>()).AsSingle();

        var eventManager = Container.Resolve<EventManager>();
        var soundManager = Container.Resolve<SoundManager>();
        soundManager.Initialize(eventManager.OnSoundPlayed);

    }


}
