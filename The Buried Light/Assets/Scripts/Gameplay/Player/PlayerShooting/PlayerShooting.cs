using UnityEngine;
using Zenject;
using Cysharp.Threading.Tasks;

public class PlayerShooting : MonoBehaviour
{
    [SerializeField] private Transform firePoint;
    [SerializeField] private float shootCooldown = 0.2f;

    private bool _canShoot = true;

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
        if (_inputManager.IsShooting && _canShoot)
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

        var projectile = _projectilePoolManager.GetProjectile();
        if (projectile == null)
        {
            Debug.LogError("No projectile available in pool!");
            _canShoot = true;
            return;
        }

        projectile.Activate(firePoint.position, firePoint.rotation);
        _playerEvents.NotifyPlayerShot();

        await UniTask.Delay((int)(shootCooldown * 1000));
        _canShoot = true;
    }

}
