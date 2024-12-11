using UnityEngine;
using Zenject;

public class PlayerShooting : MonoBehaviour
{
    [SerializeField] private Transform firePoint;
    [SerializeField] private float shootCooldown = 0.2f;

    private float _lastShotTime;

    // Dependencies
    private InputManager _inputManager;
    private ProjectilePoolManager _projectilePoolManager;
    private PlayerEvents _playerEvents;

    [Inject]
    public void Construct(InputManager inputManager, ProjectilePoolManager projectilePoolManager, PlayerEvents playerEvents)
    {
        _inputManager = inputManager;
        _projectilePoolManager = projectilePoolManager;
        _playerEvents = playerEvents;
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

        // Set the projectile's position and rotation
        projectile.transform.position = firePoint.position;
        projectile.transform.rotation = firePoint.rotation;
        projectile.SetActive(true);

        // Notify other systems about the player shooting
        _playerEvents.NotifyPlayerShot();
    }
}
