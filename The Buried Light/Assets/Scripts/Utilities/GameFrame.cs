using UnityEngine;

public class GameFrame : MonoBehaviour
{
    [Header("Boundary References")]
    [SerializeField] private Transform lowerLeftCorner;
    [SerializeField] private Transform upperRightCorner;

    [Header("Offsets")]
    [Tooltip("Offset for spawn boundaries")]
    [SerializeField] private float spawnBoundaryOffset = 0f;
    [Tooltip("Offset for wrapping boundaries")]
    [SerializeField] private float wrappingBoundaryOffset = 0.3f;

    // Properties for wrapping boundaries
    public Vector2 WrapMinBounds => MinBounds - new Vector2(wrappingBoundaryOffset, wrappingBoundaryOffset);
    public Vector2 WrapMaxBounds => MaxBounds + new Vector2(wrappingBoundaryOffset, wrappingBoundaryOffset);

    // Properties for spawn boundaries
    public Vector2 SpawnMinBounds => MinBounds + new Vector2(spawnBoundaryOffset, spawnBoundaryOffset);
    public Vector2 SpawnMaxBounds => MaxBounds - new Vector2(spawnBoundaryOffset, spawnBoundaryOffset);

    // Base frame boundaries
    public Vector2 MinBounds => lowerLeftCorner.position;
    public Vector2 MaxBounds => upperRightCorner.position;

    /// <summary>
    /// Generates a random position inside the spawn boundaries.
    /// </summary>
    public Vector2 GetRandomPositionInsideFrame()
    {
        float x = Random.Range(SpawnMinBounds.x, SpawnMaxBounds.x);
        float y = Random.Range(SpawnMinBounds.y, SpawnMaxBounds.y);

        return new Vector2(x, y);
    }

    /// <summary>
    /// Generates a random position outside the spawn boundaries.
    /// Ensures the spawn point is aimed toward the playable area.
    /// </summary>
    public Vector2 GetRandomPositionOutsideSpawnFrame()
    {
        float x, y;

        // Spawn outside horizontal boundaries
        if (Random.value < 0.5f)
        {
            x = Random.value < 0.5f ? MinBounds.x - spawnBoundaryOffset : MaxBounds.x + spawnBoundaryOffset;
            y = Random.Range(MinBounds.y - wrappingBoundaryOffset, MaxBounds.y + wrappingBoundaryOffset);
        }
        // Spawn outside vertical boundaries
        else
        {
            y = Random.value < 0.5f ? MinBounds.y - spawnBoundaryOffset : MaxBounds.y + spawnBoundaryOffset;
            x = Random.Range(MinBounds.x - wrappingBoundaryOffset, MaxBounds.x + wrappingBoundaryOffset);
        }

        return new Vector2(x, y);
    }

    /// <summary>
    /// Wraps a position around the wrapping boundaries.
    /// </summary>
    public Vector3 WrapPosition(Vector3 position)
    {
        float wrappedX = position.x;
        float wrappedY = position.y;

        if (position.x < WrapMinBounds.x)
            wrappedX = WrapMaxBounds.x;
        else if (position.x > WrapMaxBounds.x)
            wrappedX = WrapMinBounds.x;

        if (position.y < WrapMinBounds.y)
            wrappedY = WrapMaxBounds.y;
        else if (position.y > WrapMaxBounds.y)
            wrappedY = WrapMinBounds.y;

        return new Vector3(wrappedX, wrappedY, position.z);
    }
}
