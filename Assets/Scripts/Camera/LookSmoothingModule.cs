using UnityEngine;

public class LookSmoothingModule : ICameraMotionModule
{
    #region Parameter

    private static readonly Vector3 Zero = Vector3.zero;
    private readonly float _sensitivity;
    private readonly float _clampAngle;


    private float _smoothTime = 0.05f;

    // --- State ---
    private float _targetYaw;
    private float _targetPitch;

    private float _currentYaw;
    private float _currentPitch;

    private float _yawVelocity;
    private float _pitchVelocity;

    public Quaternion _lookRotation { get; private set; }

    [System.Serializable]
    public class Settings
    {
        [Range(0.01f, 0.2f)] public float smoothTime = 0.05f;
    }

    #region Setup

    public LookSmoothingModule(PlayerStatsSO stats)
    {
        if (stats == null) return;

        _sensitivity = stats.baseSensitive;
        _clampAngle = stats.lookLimit;

        _currentYaw = 0f;
        _currentPitch = 0f;
        _targetYaw = 0f;
        _targetPitch = 0f;
    }

    #endregion

    #endregion

    #region ICameraMotionModule
    public void Tick(float deltaTime, CameraMotionData data)
    {
        _targetYaw += data.MouseDelta.x * _sensitivity * deltaTime;
        _targetPitch += data.MouseDelta.y * _sensitivity * deltaTime;
        _targetPitch = Mathf.Clamp(_targetPitch, -_clampAngle, _clampAngle);

        _currentYaw = Mathf.SmoothDamp(_currentYaw, _targetYaw, ref _yawVelocity, _smoothTime);
        _currentPitch = Mathf.SmoothDamp(_currentPitch, _targetPitch, ref _pitchVelocity, _smoothTime);

        _lookRotation = Quaternion.Euler(-_currentPitch, _currentYaw, 0f);
    }

    public Vector3 GetPositionOffset()
    {
        return Zero;
    }

    public Vector3 GetRotationOffset()
    {
        return Zero;
    }
    #endregion

    /// <summary>Chỉ Yaw hiện tại (dùng để xoay body của player).</summary>
    public float GetCurrentYaw() => _currentYaw;

    /// <summary>Chỉ Pitch hiện tại (dùng để apply lên camera local rotation).</summary>
    public float GetCurrentPitch() => _currentPitch;

}