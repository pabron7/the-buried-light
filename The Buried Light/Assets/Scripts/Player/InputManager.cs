using Zenject;
using UnityEngine;

public class InputManager : ITickable
{
    public float RotationInput { get; private set; }
    public bool IsAccelerating { get; private set; }
    public bool IsShooting { get; private set; }
    public bool IsUsingSpecialMove { get; private set; }

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

        // Shooting and special moves
        IsShooting = Input.GetKey(KeyCode.Space) || Input.GetMouseButton(0);
        IsUsingSpecialMove = Input.GetKey(KeyCode.LeftShift) || Input.GetMouseButton(1);
    }
}
