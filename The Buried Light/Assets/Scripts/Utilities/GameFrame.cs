using UnityEngine;

public class GameFrame : MonoBehaviour
{
    [Header("Offsets")]
    [Tooltip("Offset for spawn boundaries")]
    [SerializeField] private float spawnBoundaryOffset = 0f;
    [Tooltip("Offset for wrapping boundaries")]
    [SerializeField] private float wrappingBoundaryOffset = 0.3f;

    private Camera _mainCamera;
    private Vector2 _minBounds;
    private Vector2 _maxBounds;

    // Properties for wrapping boundaries
    public Vector2 WrapMinBounds => _minBounds - new Vector2(wrappingBoundaryOffset, wrappingBoundaryOffset);
    public Vector2 WrapMaxBounds => _maxBounds + new Vector2(wrappingBoundaryOffset, wrappingBoundaryOffset);

    // Properties for spawn boundaries
    public Vector2 SpawnMinBounds => _minBounds + new Vector2(spawnBoundaryOffset, spawnBoundaryOffset);
    public Vector2 SpawnMaxBounds => _maxBounds - new Vector2(spawnBoundaryOffset, spawnBoundaryOffset);

    // Base frame boundaries
    public Vector2 MinBounds => _minBounds;
    public Vector2 MaxBounds => _maxBounds;

    private void Awake()
    {
        _mainCamera = Camera.main;
        if (_mainCamera == null)
        {
            Debug.LogError("GameFrame: No Main Camera found!");
            return;
        }

        AdjustToCamera();
    }

    /// <summary>
    /// Adjusts the frame to match the camera's viewport dynamically.
    /// </summary>
    private void AdjustToCamera()
    {
        Vector3 lowerLeft = _mainCamera.ViewportToWorldPoint(new Vector3(0, 0, 0));
        Vector3 upperRight = _mainCamera.ViewportToWorldPoint(new Vector3(1, 1, 0));

        _minBounds = new Vector2(lowerLeft.x, lowerLeft.y);
        _maxBounds = new Vector2(upperRight.x, upperRight.y);
    }

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
