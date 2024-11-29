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
        // Apply movement
        Vector2 movement = _inputManager.MovementInput;
        transform.Translate(new Vector3(movement.x, movement.y, 0) * moveSpeed * Time.deltaTime);

        // Apply rotation
        float rotation = _inputManager.RotationInput;
        transform.Rotate(0, 0, -rotation * rotationSpeed * Time.deltaTime); // Negative for clockwise rotation
    }
}
