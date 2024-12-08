using UnityEngine;

[CreateAssetMenu(fileName = "NewPlayerControllerConfig", menuName = "Game/Player Controller Config")]
public class PlayerControllerConfig : ScriptableObject
{
    [Tooltip("The force applied when the player moves forward.")]
    public float acceleration = 5f;

    [Tooltip("The speed at which the player rotates.")]
    public float rotationSpeed = 200f;

    [Tooltip("The maximum speed the player can reach.")]
    public float maxSpeed = 10f;

    [Tooltip("The drag applied to slow down the player's movement.")]
    public float drag = 0.5f;
}
