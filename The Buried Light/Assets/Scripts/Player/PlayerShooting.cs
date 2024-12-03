using UnityEngine;
using Zenject;

public class PlayerShooting : MonoBehaviour
{
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float shootCooldown = 0.2f;

    private float _lastShotTime;
    private InputManager _inputManager;

    [Inject]
    public void Construct(InputManager inputManager)
    {
        _inputManager = inputManager;
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
        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
        projectile.transform.up = firePoint.up; // Ensure the projectile fires forward
    }
}

