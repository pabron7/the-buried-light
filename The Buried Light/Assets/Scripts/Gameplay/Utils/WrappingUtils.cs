using UnityEngine;
using System;
using Zenject;

public class WrappingUtils
{
    private readonly GameFrame _gameFrame;

    [Inject]
    public WrappingUtils(GameFrame gameFrame)
    {
        _gameFrame = gameFrame ?? throw new ArgumentNullException(nameof(gameFrame));
    }

    /// <summary>
    /// Wraps a position around the game frame boundaries.
    /// </summary>
    public Vector3 WrapPosition(Vector3 position)
    {
        if (_gameFrame == null) return position;

        float wrappedX = position.x;
        float wrappedY = position.y;

        if (position.x < _gameFrame.MinBounds.x)
            wrappedX = _gameFrame.MaxBounds.x;
        else if (position.x > _gameFrame.MaxBounds.x)
            wrappedX = _gameFrame.MinBounds.x;

        if (position.y < _gameFrame.MinBounds.y)
            wrappedY = _gameFrame.MaxBounds.y;
        else if (position.y > _gameFrame.MaxBounds.y)
            wrappedY = _gameFrame.MinBounds.y;

        return new Vector3(wrappedX, wrappedY, position.z);
    }
}
