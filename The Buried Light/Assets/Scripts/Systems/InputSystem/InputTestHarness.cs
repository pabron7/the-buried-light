using UnityEngine;
using Zenject;

public class InputTestHarness : MonoBehaviour
{
    private InputManager _inputManager;

    [Inject]
    public void Construct(InputManager inputManager)
    {
        _inputManager = inputManager;
    }

    private void Update()
    {
        _inputManager.Tick();
    }
}
