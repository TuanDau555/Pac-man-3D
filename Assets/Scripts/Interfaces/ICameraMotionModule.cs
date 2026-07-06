using UnityEngine;

/// <summary>
/// Interface for all camera motion module
/// Each module ony return postion and rotation offset
/// </summary>
public interface ICameraMotionModule
{
    /// <summary>
    /// Call every frame
    /// pass the data from PlayerController
    /// </summary>
    void Tick(float deltaTime, CameraMotionData data);

    /// <summary>
    /// Return offset postion of the module
    /// </summary>
    Vector3 GetPositionOffset();

    /// <summary>
    /// Return the Euler angles offset of the module
    /// </summary>
    Vector3 GetRotationOffset();
}