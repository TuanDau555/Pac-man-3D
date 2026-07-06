using UnityEngine;

[CreateAssetMenu(fileName = "ScriptableObject/Player", menuName = "PlayerCamStatsSO")]
public class PlayerCamStatsSO : ScriptableObject
{
    #region Head bob

    [Header("-----HEAD BOB-----")]
    [Header("Walk")]
    [Range(1.4f, 1.8f)]
    public float walkFrequency = 1.6f;

    [Range(0.005f, 0.01f)]
    public float walkAmplitudeY = 0.008f;

    [Tooltip("Ellipse width (NOTE must smaller then Y about 50%)")]
    public float walkAmplitudeX = 0.004f;

    [Tooltip("Degrees")]
    public float walkRollAmount = 0.4f;
    public float walkPitchAmount = 0.3f;

    [Space]
    [Header("Sprint")]
    [Range(2.0f, 2.6f)]
    public float sprintFrequency = 2.4f;

    [Range(0.012f, 0.02f)]
    public float sprintAmplitudeY = 0.016f;

    [Tooltip("Ellipse width (NOTE must smaller then Y about 50%)")]
    public float sprintAmplitudeX = 0.008f;

    [Tooltip("Degrees")]
    public float sprintRollAmount = 0.8f;
    public float sprintPitchAmount = 0.6f;

    [Tooltip("Blend-in/out speed when starting or stopping movement")]
    public float blendSpeed = 8f;

    #endregion

    #region Acceleration

    [Header("-----ACCELERATION-----")]

    [Tooltip("Acceleration sensitive")]
    [Range(0.002f, 0.005f)]
    public float accelerationInfluence = 0.003f;

    [Tooltip("Decay effect speed")]
    [Range(5f, 8f)]
    public float decaySpeed = 6f;

    [Tooltip("Offset limit")]
    [Range(0.02f, 0.03f)]
    public float accelerateMaxPositionOffset = 0.025f;
    [Range(1f, 1.5f)]
    public float maxTiltAngle = 1.2f;

    #endregion

    #region Camera Spring

    [Header("Camera Srping")]
    [Tooltip("Restoring force (the greater it is, the stiffer the system and the faster the return)")]

    [Range(60f, 150f)]
    public float stiffness = 60f;

    [Tooltip("The higher is it the faster it come back\n" +
    "damping = 2*sqrt(stiffness*mass)")]
    [Range(10f, 22f)]
    public float damping = 10f;

    [Range(0.001f, 0.003f)]
    public float velocityInfluence = 0.0015f;

    [Range(0.0005f, 0.0015f)]
    public float mouseDeltaInfluence = 0.0008f;

    [Range(0.03f, 0.06f)]
    public float springMaxPositionOffset = 0.04f;

    [Tooltip("Degree")]
    [Range(1f, 2f)]
    public float maxRotationOffset = 1.5f;

    #endregion
}
