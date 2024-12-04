using UnityEngine;
using Zenject;

public class PlayerShooting : MonoBehaviour
{
    [SerializeField] private Transform firePoint;
    [SerializeField] private float shootCooldown = 0.2f;

    private float _lastShotTime;
    private InputManager _inputManager;
    private ProjectilePoolManager _projectilePoolManager;

    [Inject]
    public void Construct(InputManager inputManager, ProjectilePoolManager projectilePoolManager)
    {
        _inputManager = inputManager;
        _projectilePoolManager = projectilePoolManager;
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
        projectile.transform.position = firePoint.position;
        projectile.transform.rotation = firePoint.rotation;

        var projectileComponent = projectile.GetComponent<IProjectile>();
        if (projectileComponent == null)
        {
            Debug.LogError("Projectile does not implement IProjectile!");
        }

        projectile.SetActive(true);
    }
}
