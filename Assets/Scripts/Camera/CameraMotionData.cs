using UnityEngine;

/// <summary>
/// Input data from PlayerController send to CameraMotionController every frame
/// NOTE: PlayerController just provide data
/// </summary>
public struct CameraMotionData
{
    /// <summary>
    /// Current velocity of player (CharacterController I think?)
    /// </summary>
    public Vector3 Velocity;

    /// <summary>
    /// To calculate historical interpolation
    /// </summary>
    public Vector3 PreviousVelocity;

    public Vector2 MoveInput;

    public Vector2 MouseDelta;

    public bool IsGrounded;

    public bool IsSprinting;
}