using UnityEngine;

public class FrameConstraint : MonoBehaviour
{
    [SerializeField] private GameFrame gameFrame;

    private void Update()
    {
        Vector2 minBounds = gameFrame.MinBounds;
        Vector2 maxBounds = gameFrame.MaxBounds;

        // Clamp position within frame bounds
        Vector3 clampedPosition = new Vector3(
            Mathf.Clamp(transform.position.x, minBounds.x, maxBounds.x),
            Mathf.Clamp(transform.position.y, minBounds.y, maxBounds.y),
            transform.position.z
        );

        transform.position = clampedPosition;
    }
}
