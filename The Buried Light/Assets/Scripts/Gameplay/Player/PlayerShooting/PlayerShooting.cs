using UnityEngine;
using Zenject;
using Cysharp.Threading.Tasks;

public class PlayerShooting : MonoBehaviour
{
    [SerializeField] private Transform firePoint;
    [SerializeField] private float shootCooldown = 0.2f;

    private bool _canShoot = true;

    // Dependencies with LazyInject
    private LazyInject<InputManager> _inputManager;
    private LazyInject<ProjectilePoolManager> _projectilePoolManager;
    private LazyInject<PlayerEvents> _playerEvents;

    [Inject]
    public void Construct(
        LazyInject<InputManager> inputManager,
        LazyInject<ProjectilePoolManager> projectilePoolManager,
        LazyInject<PlayerEvents> playerEvents)
    {
        _inputManager = inputManager;
        _projectilePoolManager = projectilePoolManager;
        _playerEvents = playerEvents;
    }

    private void Update()
    {
        // Ensure the InputManager is resolved before using it
        if (_inputManager.Value != null && _inputManager.Value.IsShooting && _canShoot)
        {
            Shoot().Forget();
        }
    }

    /// <summary>
    /// Handles the shooting logic asynchronously, including cooldown management.
    /// </summary>
    private async UniTaskVoid Shoot()
    {
        _canShoot = false;

        var projectilePool = _projectilePoolManager.Value;
        if (projectilePool == null)
        {
            Debug.LogError("ProjectilePoolManager is not resolved!");
            _canShoot = true;
            return;
        }

        var projectile = projectilePool.GetProjectile();
        if (projectile == null)
        {
            Debug.LogError("No projectile available in pool!");
            _canShoot = true;
            return;
        }

        projectile.Activate(firePoint.position, firePoint.rotation);

        var playerEvents = _playerEvents.Value;
        if (playerEvents != null)
        {
            playerEvents.NotifyPlayerShot();
        }
        else
        {
            Debug.LogWarning("PlayerEvents is not resolved!");
        }

        await UniTask.Delay((int)(shootCooldown * 1000));
        _canShoot = true;
    }
}
