using UnityEngine;

public class AccelerationModule : ICameraMotionModule
{
    #region Parameters

    private PlayerCamStatsSO statsSO;

    private float _accelerationInfluence;
    private float _decaySpeed;
    private float _maxPositionOffset;
    private float _maxTiltAngle;

    // --- State ---
    private Vector3 _smoothedAcceleration = Vector3.zero;

    private Vector3 _positionOffset = Vector3.zero;
    private Vector3 _rotationOffset = Vector3.zero;

    #endregion

    #region Constructor

    public AccelerationModule(PlayerCamStatsSO playerCamStatsSO)
    {
        statsSO = playerCamStatsSO;

        Init();
    }

    #endregion

    #region Init

    private void Init()
    {
        _accelerationInfluence = statsSO.accelerationInfluence;
        _decaySpeed = statsSO.decaySpeed;
        _maxPositionOffset = statsSO.accelerateMaxPositionOffset;
        _maxTiltAngle = statsSO.maxTiltAngle;
    }

    #endregion

    #region ICameraMotionModule

    public void Tick(float deltaTime, CameraMotionData data)
    {
        Vector3 rawAcceleration = data.Velocity - data.PreviousVelocity;
        rawAcceleration.y = 0f;

        _smoothedAcceleration = Vector3.Lerp(
            _smoothedAcceleration,
            rawAcceleration,
            _decaySpeed * deltaTime
        );

        // The camera position shifts backward during acceleration and forward during braking.
        float forwardOffset = -_smoothedAcceleration.magnitude * _accelerationInfluence;

        Vector3 rawPositionOffset = new Vector3(0f, -forwardOffset * 0.5f, -forwardOffset);
        _positionOffset = Vector3.ClampMagnitude(rawPositionOffset, _maxPositionOffset);

        // Rotation: tilt slightly towards the direction of acceleration (lean into it)
        // Pitch down when accelerating forward, pitch up when braking
        float pitchTilt = -_smoothedAcceleration.magnitude * _accelerationInfluence * 40f;
        pitchTilt = Mathf.Clamp(pitchTilt, -_maxTiltAngle, _maxTiltAngle);

        _rotationOffset = new Vector3(pitchTilt, 0f, 0f);
    }

    public Vector3 GetPositionOffset() => _positionOffset;
    public Vector3 GetRotationOffset() => _rotationOffset;

    #endregion
}