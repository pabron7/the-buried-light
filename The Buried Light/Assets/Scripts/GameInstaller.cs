using System;
using Zenject;
using UnityEngine;

public class GameInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        //Systems
        Container.Bind<GameManager>().AsSingle();
        Container.Bind<LevelManager>().AsSingle();
        Container.BindInterfacesAndSelfTo<InputManager>().AsSingle();
        Container.BindInterfacesAndSelfTo<GameLoop>().AsSingle();
        Container.Bind<InputTestHarness>().FromNewComponentOnNewGameObject().AsSingle();

        //Player Controls
        Container.Bind<PlayerMovement>().FromComponentInHierarchy().AsSingle();
        Container.Bind<PlayerShooting>().FromComponentInHierarchy().AsSingle();
        Container.Bind<SpecialMove>().FromComponentInHierarchy().AsSingle();
    }
}
