using UnityEngine;

/// <summary>
/// Handles enemy movement and wrapping logic.
/// </summary>
public class EnemyMovement : MonoBehaviour
{
    private float _speed;
    private Vector3 _direction;
    private WrappingUtils _wrappingUtils;
    private GameFrame _gameFrame;

    /// <summary>
    /// Initializes the movement with speed and direction.
    /// </summary>
    public void Initialize(float speed, Vector3 direction)
    {
        _speed = speed;
        _direction = direction.normalized;
    }

    /// <summary>
    /// Sets dependencies required for wrapping logic.
    /// </summary>
    public void SetDependencies(WrappingUtils wrappingUtils, GameFrame gameFrame)
    {
        _wrappingUtils = wrappingUtils;
        _gameFrame = gameFrame;
    }

    /// <summary>
    /// Moves the enemy based on speed and direction.
    /// </summary>
    public void Move()
    {
        transform.position += _direction * _speed * Time.deltaTime;
    }

    /// <summary>
    /// Wraps the enemy around the screen if it goes out of bounds.
    /// </summary>
    public void WrapIfOutOfBounds()
    {
        if (_wrappingUtils != null && _gameFrame != null)
        {
            transform.position = _wrappingUtils.WrapPosition(transform.position);
        }
    }
}
