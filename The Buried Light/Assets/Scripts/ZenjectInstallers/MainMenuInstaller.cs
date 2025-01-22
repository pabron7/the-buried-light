using Zenject;
using UnityEngine;

public class MainMenuInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        //UI Installers
        Container.Bind<TabsController>().FromComponentInHierarchy().AsSingle();
    }

    private void OnDestroy()
    {
        Debug.Log("MainMenu Installer is being destroyed!");
    }
}
