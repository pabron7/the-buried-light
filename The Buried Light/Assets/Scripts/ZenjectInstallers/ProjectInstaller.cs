using UnityEngine;
using Zenject;

public class ProjectInstaller : MonoInstaller
{
    [SerializeField] private SoundRegistry soundRegistry;
    [SerializeField] private VFXRegistry vFXRegistry;

    [SerializeField] private GameObject gameManagerPrefab;
    [SerializeField] private GameObject sceneManagerPrefab;
    [SerializeField] private GameObject soundManagerPrefab;
    [SerializeField] private GameObject uiManagerPrefab;
    [SerializeField] private GameObject vfxManagerPrefab;
    [SerializeField] private GameObject musicManagerPrefab;


    public override void InstallBindings()
    {
        // Game Manager and Events
        Container.Bind<GameManager>().FromComponentInNewPrefab(gameManagerPrefab).AsSingle().NonLazy();
        Container.Bind<SceneManager>().FromComponentInNewPrefab(sceneManagerPrefab).AsSingle().NonLazy();
        Container.Bind<GameEvents>().AsSingle();
        Container.Bind<EnemyEvents>().AsSingle();
        Container.Bind<PlayerEvents>().AsSingle();

        // Input Systems
        Container.BindInterfacesAndSelfTo<InputManager>().AsSingle();

        // Sound Systems
        Container.Bind<SoundRegistry>().FromInstance(soundRegistry).AsSingle();
        Container.Bind<SoundManager>().FromComponentInNewPrefab(soundManagerPrefab).AsSingle().NonLazy();
        Container.Bind<Playlist>().AsSingle();
        Container.Bind<MusicPlayer>().AsSingle();
        Container.Bind<MusicManager>().FromComponentInNewPrefab(musicManagerPrefab).AsSingle().NonLazy();

        // VFX Systems
        Container.Bind<VFXRegistry>().FromInstance(vFXRegistry).AsSingle();
        Container.Bind<VFXManager>().FromComponentInNewPrefab(vfxManagerPrefab).AsSingle().NonLazy();

        // Score
        Container.Bind<ScoreManager>().AsSingle();

        // UI Systems
        Container.Bind<UIManager>().FromComponentInNewPrefab(uiManagerPrefab).AsSingle().NonLazy();
    }
}
