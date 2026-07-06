using UnityEngine;

public class HeadBobModule : ICameraMotionModule
{
    #region Parameters

    private PlayerCamStatsSO _playerStats;

    // --- State ---
    private float _bobTimer = 0f;
    private float _currentBlend = 0f; // 0 is stop and 1 is moving

    // local variables
    private float _walkFrequency;
    private float _walkAmplitudeY;
    private float _walkAmplitudeX;
    private float _walkRollAmount;
    private float _walkPitchAmount;
    private float _sprintFrequency;
    private float _sprintAmplitudeY;
    private float _sprintAmplitudeX;
    private float _sprintRollAmount;
    private float _sprintPitchAmount;
    private float _blendSpeed;

    private Vector3 _positionOffset = Vector3.zero;
    private Vector3 _rotationOffset = Vector3.zero;

    #endregion

    #region Constructor

    public HeadBobModule(PlayerCamStatsSO playerCamStatsSO)
    {
        _playerStats = playerCamStatsSO;

        Init();
    }

    #endregion

    #region Init

    private void Init()
    {
        _walkFrequency = _playerStats.walkFrequency;
        _walkAmplitudeY = _playerStats.walkAmplitudeY;
        _walkAmplitudeX = _playerStats.walkAmplitudeX;
        _walkRollAmount = _playerStats.walkRollAmount;
        _walkPitchAmount = _playerStats.walkPitchAmount;

        _sprintFrequency = _playerStats.sprintFrequency;
        _sprintAmplitudeY = _playerStats.sprintAmplitudeY;
        _sprintAmplitudeX = _playerStats.sprintAmplitudeX;
        _sprintRollAmount = _playerStats.sprintRollAmount;
        _sprintPitchAmount = _playerStats.sprintPitchAmount;

        _blendSpeed = _playerStats.blendSpeed;
    }

    #endregion

    #region ICameraMotionModule

    public void Tick(float deltaTime, CameraMotionData data)
    {
        // Determine if player is moving on the ground or not
        float horizontalSpeed = new Vector2(data.Velocity.x, data.Velocity.z).magnitude;
        bool isMoving = data.IsGrounded && horizontalSpeed > 0.1f;

        float targetBlend = isMoving ? 1f : 0f;
        _currentBlend = Mathf.Lerp(_currentBlend, targetBlend, _blendSpeed * deltaTime);

        if (_currentBlend < 0.001f)
        {
            // rest to center when blend move back to 0
            _positionOffset = Vector3.Lerp(_positionOffset, Vector3.zero, 10f * deltaTime);
            _rotationOffset = Vector3.Lerp(_rotationOffset, Vector3.zero, 10f * deltaTime);
            return;
        }

        // Store to use for sprint or walk setting
        float frequency = 0;
        float amplitudeY = 0;
        float amplitudeX = 0;
        float rollAmount = 0;
        float pitchAmount = 0;

        if (data.IsSprinting)
        {
            frequency = _sprintFrequency;
            amplitudeX = _sprintAmplitudeX;
            amplitudeY = _sprintAmplitudeY;
            rollAmount = _sprintRollAmount;
            pitchAmount = _sprintPitchAmount;
        }
        else
        {
            frequency = _walkFrequency;
            amplitudeX = _walkAmplitudeX;
            amplitudeY = _walkAmplitudeY;
            rollAmount = _walkRollAmount;
            pitchAmount = _walkPitchAmount;
        }

        // Increase timer by frequency
        _bobTimer += deltaTime * frequency * Mathf.PI * 2f;

        // Ellipse bob X = cos, Y = sin
        float sinValue = Mathf.Sin(_bobTimer);
        float cosValue = Mathf.Cos(_bobTimer);

        float bobX = cosValue * amplitudeX;
        float bobY = (sinValue < 0 ? sinValue : Mathf.Abs(sinValue)) * amplitudeY;

        // Roll is leaning by cos (peak right/left each step)
        float roll = cosValue * rollAmount;

        // Pitch is slight nodding by sin (dipping with each step)
        float pitch = -Mathf.Abs(sinValue) * pitchAmount;

        // Apply blend
        _positionOffset = new Vector3(bobX, bobY, 0f) * _currentBlend;
        _rotationOffset = new Vector3(pitch, 0f, roll) * _currentBlend;
    }

    public Vector3 GetPositionOffset() => _positionOffset;
    public Vector3 GetRotationOffset() => _rotationOffset;

    #endregion
}