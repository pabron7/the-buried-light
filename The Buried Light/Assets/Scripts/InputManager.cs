using Zenject;
using UnityEngine;

public class InputManager : ITickable
{
    public Vector2 MovementInput { get; private set; }
    public float RotationInput { get; private set; }
    public bool IsShooting { get; private set; }
    public bool IsUsingSpecialMove { get; private set; }

    public void Tick()
    {

        MovementInput = new Vector2(
            Input.GetAxis("Horizontal"), 
            Input.GetAxis("Vertical")   
        );

   
        if (Input.GetKey(KeyCode.J))
        {
            RotationInput = -1f; 
        }
        else if (Input.GetKey(KeyCode.L))
        {
            RotationInput = 1f; 
        }
        else
        {
            RotationInput = 0f;
        }

        // Mouse Rotation (optional if mouse controls rotation)
        if (Input.GetMouseButton(0))
        {
            RotationInput = Input.GetAxis("Mouse X"); // Horizontal mouse movement
        }

        // Shooting Inputs
        IsShooting = Input.GetKey(KeyCode.I) || Input.GetMouseButton(0);

        // Special Move (Tome)
        IsUsingSpecialMove = Input.GetKey(KeyCode.K) || Input.GetMouseButton(1);

        // Debug.Log($"Movement: {MovementInput}, Rotation: {RotationInput}, Shooting: {IsShooting}, SpecialMove: {IsUsingSpecialMove}");
    }
}


