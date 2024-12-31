using Zenject;
using UnityEngine;

public class MainMenuInstaller : MonoInstaller
{

    public override void InstallBindings()
    {
        Debug.Log("MainMenu is Installed!");
    }

    private void OnDestroy()
    {
        Debug.Log("MainMenu Installer is being destroyed!");
    }
}
