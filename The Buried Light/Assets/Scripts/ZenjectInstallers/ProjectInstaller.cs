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

    private string message = "Project Installer is binding dependencies...";

    public override void InstallBindings()
    {

        Debug.Log($"<color=green>[Zenject]</color> {message}");
        // Game Manager and Events
        Container.Bind<GameManager>().FromComponentInNewPrefab(gameManagerPrefab).AsSingle().NonLazy();
        Container.Bind<SceneManager>().FromComponentInNewPrefab(sceneManagerPrefab).AsSingle().NonLazy();
        Container.Bind<GameEvents>().AsSingle().NonLazy();
        Container.Bind<EnemyEvents>().AsSingle();
        Container.Bind<PlayerEvents>().AsSingle();

        // Save System
        Container.BindInterfacesAndSelfTo<SaveManager>().AsSingle().NonLazy();
        Container.Bind<ISaveHandler>().To<SaveHandler>().AsSingle();
        Container.Bind<ILoadHandler>().To<LoadHandler>().AsSingle();
        Container.Bind<IFileWriter>().To<FileWriter>().AsSingle();
        Container.Bind<IFileReader>().To<FileReader>().AsSingle();
        Container.BindInterfacesAndSelfTo<SaveEventHandler>().AsSingle().NonLazy();


        // Data Stores
        Container.Bind<PlayerPreferencesStore>().AsSingle().NonLazy();
        Container.Bind<PlayerStatsStore>().AsSingle().NonLazy();
        Container.Bind<PlayerDataStore>().AsSingle().NonLazy();
        Container.Bind<GameProgressStore>().AsSingle().NonLazy();

        // Input Systems
        Container.BindInterfacesAndSelfTo<InputManager>().AsSingle();

        // Sound Systems
        Container.Bind<SoundManager>().FromComponentInNewPrefab(soundManagerPrefab).AsSingle().NonLazy();
        Container.Bind<SoundRegistry>().FromComponentInChildren().AsSingle().NonLazy();
        Container.Bind<Playlist>().AsSingle();
        Container.Bind<MusicPlayer>().AsSingle();
        Container.Bind<MusicManager>().FromComponentInNewPrefab(musicManagerPrefab).AsSingle().NonLazy();

        // VFX Systems
        Container.Bind<VFXManager>().FromComponentInNewPrefab(vfxManagerPrefab).AsSingle().NonLazy();
        Container.Bind<VFXRegistry>().FromComponentInChildren().AsSingle().NonLazy();

        // Score
        Container.Bind<IResetScore>().To<ResetScore>().AsTransient();
        Container.Bind<IAddScore>().To<AddScore>().AsTransient();
        Container.Bind<ScoreManager>().AsSingle();

        // UI Systems
        Container.Bind<UIManager>().FromComponentInNewPrefab(uiManagerPrefab).AsSingle().NonLazy();
        Container.Bind<UILoader>().AsSingle();

        Debug.Log("ProjectInstaller: Bindings completed.");
    }
}
