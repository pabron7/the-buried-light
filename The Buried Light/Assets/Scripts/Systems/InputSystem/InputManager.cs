using Zenject;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System;

public class InputManager : ITickable
{
    public float RotationInput { get; private set; }
    public bool IsAccelerating { get; private set; }
    public bool IsShooting { get; private set; }
    public bool IsUsingSpecialMove { get; private set; }

    private bool _canShoot = true;
    private const float ShootCooldown = 0.2f;

    private Joystick _joystick;
    private bool _isJoystickEnabled;

    public InputManager()
    {
        _isJoystickEnabled = false;
    }

    public void SetJoystick(Joystick joystick)
    {
        _joystick = joystick;
        _isJoystickEnabled = joystick != null;

        if (_joystick != null)
        {
            _joystick.OnJoystickPressed += HandleJoystickPressed;
            _joystick.OnJoystickReleased += HandleJoystickReleased;
        }
    }

    public void Tick()
    {
        if (_isJoystickEnabled && _joystick != null)
        {
            ReadJoystickInput();
        }
        else
        {
            ReadKeyboardInput();
        }
    }

    private void ReadKeyboardInput()
    {
        // Rotation input (A/D keys)
        if (Input.GetKey(KeyCode.A))
        {
            RotationInput = -1f;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            RotationInput = 1f;
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
                HandleShooting().Forget();
            }
        }
        else
        {
            IsShooting = false;
        }

        // Special move input
        IsUsingSpecialMove = Input.GetKey(KeyCode.LeftShift) || Input.GetMouseButton(1);
    }

    private void ReadJoystickInput()
    {
        // Rotation input
        RotationInput = _joystick.Horizontal;

        // Acceleration input
        IsAccelerating = _joystick.Vertical > 0.5f;

        // Shooting input (continuous shooting while joystick is moved)
        if (_joystick.Vertical > 0.5f || _joystick.Horizontal > 0.5f || _joystick.Vertical < -0.5f || _joystick.Horizontal < -0.5f)
        {
            if (_canShoot)
            {
                IsShooting = true;
                HandleShooting().Forget();
            }
        }
        else
        {
            IsShooting = false;
        }
    }

    private void HandleJoystickPressed()
    {
        // Immediate shooting on joystick press
        if (_canShoot)
        {
            IsShooting = true;
            HandleShooting().Forget();
        }
    }

    private void HandleJoystickReleased()
    {
        // Stop shooting when joystick is released
        IsShooting = false;
    }

    private async UniTaskVoid HandleShooting()
    {
        _canShoot = false;

        // Wait for the cooldown period
        await UniTask.Delay((int)(ShootCooldown * 1000));

        _canShoot = true;
    }
}
