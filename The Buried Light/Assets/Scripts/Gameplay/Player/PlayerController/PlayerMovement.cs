using UnityEngine;
using Zenject;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private PlayerControllerConfig controllerConfig; 

    private Rigidbody2D _rigidbody;
    private InputManager _inputManager;

    [Inject]
    public void Construct(InputManager inputManager)
    {
        _inputManager = inputManager;
    }

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        HandleRotation();
        HandleMovement();
        ApplyDrag();
        LimitSpeed();
    }

    private void HandleRotation()
    {
        float rotationInput = _inputManager.RotationInput;
        _rigidbody.rotation -= rotationInput * controllerConfig.rotationSpeed * Time.fixedDeltaTime;
    }

    private void HandleMovement()
    {
        if (_inputManager.IsAccelerating)
        {
            Vector2 forwardDirection = transform.up;
            _rigidbody.AddForce(forwardDirection * controllerConfig.acceleration, ForceMode2D.Force);
        }
    }

    private void ApplyDrag()
    {
        if (_rigidbody.velocity.sqrMagnitude > 0)
        {
            _rigidbody.AddForce(-_rigidbody.velocity * controllerConfig.drag, ForceMode2D.Force);
        }
    }

    private void LimitSpeed()
    {
        if (_rigidbody.velocity.sqrMagnitude > controllerConfig.maxSpeed * controllerConfig.maxSpeed)
        {
            _rigidbody.velocity = _rigidbody.velocity.normalized * controllerConfig.maxSpeed;
        }
    }
}
