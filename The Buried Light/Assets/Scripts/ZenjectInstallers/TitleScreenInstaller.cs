using Zenject;
using UnityEngine;

public class TitleScreenInstaller : MonoInstaller
{

    public override void InstallBindings()
    {
  
    }

    private void OnDestroy()
    {
        Debug.Log("Title Screen Installer is destroyed!");
    }
}
