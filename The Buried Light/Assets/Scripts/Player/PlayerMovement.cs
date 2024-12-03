using UnityEngine;
using Zenject;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float rotationSpeed = 200f;

    private InputManager _inputManager;

    [Inject]
    public void Construct(InputManager inputManager)
    {
        _inputManager = inputManager;
    }

    private void Update()
    {
        Vector2 movement = _inputManager.MovementInput;
        Vector3 moveDirection = new Vector3(movement.x, movement.y, 0) * moveSpeed * Time.deltaTime;
        transform.position += moveDirection;

        // Rotation (J/L or Mouse for aiming)
        float rotation = _inputManager.RotationInput;
        transform.Rotate(0, 0, -rotation * rotationSpeed * Time.deltaTime); 
    }
}

