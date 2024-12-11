using Zenject;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class InputManager : ITickable
{
    public float RotationInput { get; private set; }
    public bool IsAccelerating { get; private set; }
    public bool IsShooting { get; private set; }
    public bool IsUsingSpecialMove { get; private set; }

    private bool _canShoot = true;
    private const float ShootCooldown = 0.2f;

    public void Tick()
    {
        // Rotation input (A/D keys)
        if (Input.GetKey(KeyCode.A))
        {
            RotationInput = -1f; // Clockwise in 2D
        }
        else if (Input.GetKey(KeyCode.D))
        {
            RotationInput = 1f; // Counter-clockwise in 2D
        }
        else
        {
            RotationInput = 0f;
        }

        // Acceleration input (W key)
        IsAccelerating = Input.GetKey(KeyCode.W);

        // Shooting input
        if (Input.GetKey(KeyCode.Space) || Input.GetMouseButton(0))
        {
            if (_canShoot)
            {
                IsShooting = true;
                HandleShooting().Forget(); // Asynchronous shooting with cooldown
            }
        }
        else
        {
            IsShooting = false;
        }

        // Special move input
        IsUsingSpecialMove = Input.GetKey(KeyCode.LeftShift) || Input.GetMouseButton(1);
    }

    /// <summary>
    /// Handles shooting logic with cooldown asynchronously.
    /// </summary>
    private async UniTaskVoid HandleShooting()
    {
        _canShoot = false;

        // Wait for the cooldown period
        await UniTask.Delay((int)(ShootCooldown * 1000));

        _canShoot = true;
    }
}
