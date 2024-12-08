using UnityEngine;
using Zenject;

public class PlayerShooting : MonoBehaviour
{
    [SerializeField] private Transform firePoint;
    [SerializeField] private float shootCooldown = 0.2f;

    private float _lastShotTime;

    // dependencies
    private InputManager _inputManager;
    private ProjectilePoolManager _projectilePoolManager;
    private EventManager _eventManager;

    [Inject]
    public void Construct(InputManager inputManager, ProjectilePoolManager projectilePoolManager, EventManager eventManager)
    {
        _inputManager = inputManager;
        _projectilePoolManager = projectilePoolManager;
        _eventManager = eventManager;
    }
    private void Update()
    {
        if (_inputManager.IsShooting && Time.time >= _lastShotTime + shootCooldown)
        {
            Shoot();
            _lastShotTime = Time.time;
        }
    }

    private void Shoot()
    {
        GameObject projectile = _projectilePoolManager.GetProjectile();

        if (projectile == null)
        {
            Debug.LogWarning("Failed to retrieve projectile from the pool.");
            return;
        }

        var command = new PlayerShootCommand(firePoint, projectile);
        _eventManager.ExecuteCommand(command);
    }
}
