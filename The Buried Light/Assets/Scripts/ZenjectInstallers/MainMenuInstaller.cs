using Zenject;
using UnityEngine;

public class MainMenuInstaller : MonoInstaller
{

    public override void InstallBindings()
    {
 
    }

    private void OnDestroy()
    {
        Debug.Log("MainMenu Installer is being destroyed!");
    }
}
