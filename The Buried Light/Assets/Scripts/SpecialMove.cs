using UnityEngine;
using Zenject;

public class SpecialMove : MonoBehaviour
{
    [SerializeField] private float cooldown = 5f;

    private float _lastUsedTime;
    private InputManager _inputManager;

    [Inject]
    public void Construct(InputManager inputManager)
    {
        _inputManager = inputManager;
    }

    private void Update()
    {
        if (_inputManager.IsUsingSpecialMove && Time.time >= _lastUsedTime + cooldown)
        {
            ActivateSpecialMove();
            _lastUsedTime = Time.time;
        }
    }

    private void ActivateSpecialMove()
    {
        Debug.Log("Special Move Activated!");
        // Add special move logic here (e.g., clear enemies, heal player, etc.)
    }
}
