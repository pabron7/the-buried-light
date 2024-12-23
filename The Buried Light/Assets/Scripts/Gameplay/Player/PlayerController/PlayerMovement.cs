using UnityEngine;
using Zenject;
using Cysharp.Threading.Tasks;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour, IWrappable
{
    [SerializeField] private PlayerControllerConfig controllerConfig;

    private Rigidbody2D _rigidbody;
    private InputManager _inputManager;
    private GameFrame _gameFrame;
    private WrappingUtils _wrappingUtils;

    /// <summary>
    /// Injects the InputManager dependency into the PlayerMovement class.
    /// </summary>
    [Inject]
    public void Construct(InputManager inputManager, GameFrame gameFrame, WrappingUtils wrappingUtils)
    {
        _inputManager = inputManager;
        _gameFrame = gameFrame;
        _wrappingUtils = wrappingUtils;
    }

    /// <summary>
    /// Initializes the Rigidbody2D component and waits asynchronously until the InputManager is ready.
    /// </summary>
    private async void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();

        // Wait for InputManager to be ready asynchronously
        await UniTask.WaitUntil(() => _inputManager != null);
        Debug.Log("InputManager is ready.");
    }

    /// <summary>
    /// Performs all physics-based updates for the player, including rotation, movement, drag, and speed limiting.
    /// </summary>
    private void FixedUpdate()
    {
        HandleRotation();
        HandleMovement();
        ApplyDrag();
        LimitSpeed();
        WrapIfOutOfBounds();
    }

    /// <summary>
    /// Applies rotation to the player based on input and configured rotation speed.
    /// </summary>
    private void HandleRotation()
    {
        float rotationInput = _inputManager.RotationInput;
        _rigidbody.rotation -= rotationInput * controllerConfig.rotationSpeed * Time.fixedDeltaTime;
    }

    /// <summary>
    /// Applies a forward force to the player’s Rigidbody when acceleration input is detected.
    /// </summary>
    private void HandleMovement()
    {
        if (_inputManager.IsAccelerating)
        {
            Vector2 forwardDirection = transform.up;
            _rigidbody.AddForce(forwardDirection * controllerConfig.acceleration, ForceMode2D.Force);
        }
    }

    /// <summary>
    /// Applies a drag force to gradually slow down the player’s movement when in motion.
    /// </summary>
    private void ApplyDrag()
    {
        if (_rigidbody.velocity.sqrMagnitude > 0)
        {
            _rigidbody.AddForce(-_rigidbody.velocity * controllerConfig.drag, ForceMode2D.Force);
        }
    }

    /// <summary>
    /// Limits the player’s velocity to a maximum speed defined in the configuration.
    /// </summary>
    private void LimitSpeed()
    {
        if (_rigidbody.velocity.sqrMagnitude > controllerConfig.maxSpeed * controllerConfig.maxSpeed)
        {
            _rigidbody.velocity = _rigidbody.velocity.normalized * controllerConfig.maxSpeed;
        }
    }

    /// <summary>
    /// Checks if the player is out of bounds and wraps their position using the GameFrame.
    /// </summary>
    public void WrapIfOutOfBounds()
    {
        if (_gameFrame != null)
        {
            transform.position = _wrappingUtils.WrapPosition(transform.position);
        }
    }
}