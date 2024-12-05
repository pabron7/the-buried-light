using System;
using UnityEngine;

public class PlayerShootCommand : ICommand
{
    private readonly Transform _firePoint;
    private readonly GameObject _projectile;

    public PlayerShootCommand(Transform firePoint, GameObject projectile)
    {
        _firePoint = firePoint;
        _projectile = projectile;
    }

    public void Execute()
    {
        if (_firePoint == null || _projectile == null)
        {
            Debug.LogError("PlayerShootCommand: FirePoint or Projectile is null.");
            return;
        }

        // Set projectile position and rotation
        _projectile.transform.position = _firePoint.position;
        _projectile.transform.rotation = _firePoint.rotation;

        // Activate the projectile
        _projectile.SetActive(true);

        // Log shooting action (later replaced with sound/visual effects)
        Debug.Log("Player fired a projectile!");
    }
}


