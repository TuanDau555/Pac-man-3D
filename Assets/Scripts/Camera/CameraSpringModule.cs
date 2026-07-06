using UnityEngine;


public class CameraSpringModule : ICameraMotionModule
{
    #region Parameter

    private const float Mass = 1f;
    private PlayerCamStatsSO statsSO;

    private float _stiffness;
    private float _damping;
    private float _velocityInfluence;
    private float _mouseDeltaInfluence;
    private float _maxPositionOffset;
    private float _maxRotationOffset;

    // --- State---
    private Vector3 _posSpring = Vector3.zero;
    private Vector3 _posVelocity = Vector3.zero;

    // Rotation spring (Pitch = X, Roll = Z)
    private Vector3 _rotSpring = Vector3.zero;
    private Vector3 _rotVelocity = Vector3.zero;

    private Vector3 _positionOffset = Vector3.zero;
    private Vector3 _rotationOffset = Vector3.zero;

    #endregion

    #region Constructor

    public CameraSpringModule(PlayerCamStatsSO playerCamStatsSO)
    {
        statsSO = playerCamStatsSO;

        Init();
    }

    #endregion

    #region Init

    private void Init()
    {
        _stiffness = statsSO.stiffness;
        _damping = statsSO.damping;
        _velocityInfluence = statsSO.velocityInfluence;
        _mouseDeltaInfluence = statsSO.mouseDeltaInfluence;
        _maxPositionOffset = statsSO.springMaxPositionOffset;
        _maxRotationOffset = statsSO.maxRotationOffset;
    }

    #endregion

    #region ICameraMotionModule

    public void Tick(float deltaTime, CameraMotionData data)
    {

        Vector3 velocityDelta = data.Velocity - data.PreviousVelocity;
        velocityDelta.y = 0f;

        Vector2 mouseDelta = data.MouseDelta;

        // --- Force cho position spring ---
        // Velocity thay đổi → đẩy camera ngược chiều (inertia)
        Vector3 posForce = -velocityDelta * _velocityInfluence;

        // --- Force cho rotation spring ---
        Vector3 rotForce = new Vector3(
             mouseDelta.y * _mouseDeltaInfluence,   // pitch
             0f,
            -mouseDelta.x * _mouseDeltaInfluence    // roll
        );

        // --- Integrate spring (Hooke's Law + damping) ---
        _posSpring = IntegrateSpring(_posSpring, _posVelocity, posForce, deltaTime, out _posVelocity);
        _rotSpring = IntegrateSpring(_rotSpring, _rotVelocity, rotForce, deltaTime, out _rotVelocity);

        _positionOffset = Vector3.ClampMagnitude(_posSpring, _maxPositionOffset);
        _rotationOffset = Vector3.ClampMagnitude(_rotSpring, _maxRotationOffset);
    }

    /// <summary>
    /// spring damper theory
    /// F_spring = stiffness * (-x) + damping * (-v) + externalForce
    /// </summary>
    private Vector3 IntegrateSpring(Vector3 current, Vector3 vel, Vector3 externalForce, float dt, out Vector3 newVel)
    {
        Vector3 springForce = -_stiffness * current;
        Vector3 dampingForce = -_damping * vel;
        Vector3 totalForce = (springForce + dampingForce + externalForce * _stiffness) / Mass;

        newVel = vel + totalForce * dt;
        return current + newVel * dt;
    }

    public Vector3 GetPositionOffset() => _positionOffset;
    public Vector3 GetRotationOffset() => _rotationOffset;

    #endregion
}