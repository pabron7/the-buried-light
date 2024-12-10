using Zenject;
using UnityEngine;
using System;
using UniRx;

public class GameInstaller : MonoInstaller
{
    [SerializeField] private EnemyPrefabMapping[] enemyPrefabMappings;
    [SerializeField] private WaveConfig[] waveConfigs;
    [SerializeField] private ProjectilePoolManager projectilePoolManagerPrefab;
    [SerializeField] private GameObject waveManagerPrefab;

    [SerializeField] private SoundManager soundManagerPrefab;
    [SerializeField] private SoundRegistry soundRegistry;

    public override void InstallBindings()
    {
        // Game Manager
        Container.Bind<GameManager>().FromComponentInHierarchy().AsSingle().NonLazy();

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

        // WaveManager Factory
        Container.BindFactory<WaveManager, WaveManager.Factory>()
            .FromComponentInNewPrefab(waveManagerPrefab)
            .WithGameObjectName("WaveManager")
            .UnderTransformGroup("WaveManagers");

        // Bind LevelManager
        Container.Bind<LevelManager>().FromComponentInHierarchy().AsSingle();

        // Enemy Systems
        Container.Bind<EnemyPrefabMapping[]>().FromInstance(enemyPrefabMappings).AsSingle();
        Container.Bind<EnemyFactory>().AsSingle();
        Container.Bind<EnemyPoolManager>().FromComponentInHierarchy().AsSingle();
        Container.Bind<EnemySpawner>().FromComponentInHierarchy().AsSingle();

        // Player Systems
        Container.Bind<IHealth>().To<PlayerHealth>().FromInstance(FindObjectOfType<PlayerHealth>()).AsSingle();

        // UI Systems
        Container.Bind<MenuManager>().FromComponentInHierarchy().AsSingle();

    }
}