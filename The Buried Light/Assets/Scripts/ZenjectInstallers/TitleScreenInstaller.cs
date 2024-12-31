using Zenject;
using UnityEngine;

public class TitleScreenInstaller : MonoInstaller
{

    public override void InstallBindings()
    {
        Debug.Log("Title Screen is Installed!");
    }

    private void OnDestroy()
    {
        Debug.Log("Title Screen Installer is destroyed!");
    }
}
