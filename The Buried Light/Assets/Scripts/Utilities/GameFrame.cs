using UnityEngine;

public class GameFrame : MonoBehaviour
{
    [SerializeField] private Transform lowerLeftCorner;
    [SerializeField] private Transform upperRightCorner;
    [SerializeField] private float deletionBoundaryOffset = 5f; // Offset for deletion boundaries

    // Public properties to get frame boundaries
    public Vector2 MinBounds => lowerLeftCorner.position;
    public Vector2 MaxBounds => upperRightCorner.position;

    // Deletion boundaries extend beyond the frame
    public Vector2 DeletionMinBounds => MinBounds - new Vector2(deletionBoundaryOffset, deletionBoundaryOffset);
    public Vector2 DeletionMaxBounds => MaxBounds + new Vector2(deletionBoundaryOffset, deletionBoundaryOffset);

    /// <summary>
    /// Generate a random position outside the frame
    /// </summary>
    /// <returns>Vector 2</returns>
    public Vector2 GetRandomPositionOutsideFrame()
    {
        float x = Random.value < 0.5f ? Random.Range(MinBounds.x - 5, MinBounds.x - 1) : Random.Range(MaxBounds.x + 1, MaxBounds.x + 5);
        float y = Random.value < 0.5f ? Random.Range(MinBounds.y - 5, MinBounds.y - 1) : Random.Range(MaxBounds.y + 1, MaxBounds.y + 5);

        return new Vector2(x, y);
    }

    /// <summary>
    /// Generate a random position inside the frame
    /// </summary>
    /// <returns>Vector 2</returns>
    public Vector2 GetRandomPositionInsideFrame()
    {
        float x = Random.Range(MinBounds.x, MaxBounds.x);
        float y = Random.Range(MinBounds.y, MaxBounds.y);

        return new Vector2(x, y);
    }

    /// <summary>
    /// Wraps the position around the game frame.
    /// </summary>
    public Vector3 WrapPosition(Vector3 position)
    {
        float wrappedX = position.x;
        float wrappedY = position.y;

        if (position.x < MinBounds.x)
            wrappedX = MaxBounds.x;
        else if (position.x > MaxBounds.x)
            wrappedX = MinBounds.x;

        if (position.y < MinBounds.y)
            wrappedY = MaxBounds.y;
        else if (position.y > MaxBounds.y)
            wrappedY = MinBounds.y;

        return new Vector3(wrappedX, wrappedY, position.z);
    }
}
