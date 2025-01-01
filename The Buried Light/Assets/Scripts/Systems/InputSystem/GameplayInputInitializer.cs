using Zenject;
using UnityEngine;

public class GameplayInputInitializer : MonoBehaviour
{
    [SerializeField] private Joystick joystick;

    [Inject]
    private void Initialize(InputManager inputManager)
    {
        inputManager.SetJoystick(joystick);
    }
}
