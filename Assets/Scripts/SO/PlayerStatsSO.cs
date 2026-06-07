using UnityEngine;

[CreateAssetMenu(fileName = "ScriptableObject/Player", menuName = "PlayerStatsSO")]
public class PlayerStatsSO : ScriptableObject
{
    [Header("HP")]
    [Range(1, 5)]
    public int HP;

    [Space(10)]
    [Header("Stamina")]
    [Range(1, 100)]
    public float stamina;
    public bool useStamina;

    [Tooltip("Use stamina value")]
    [Range(1, 10)]
    public float staminaUseMultiplier = 5f;
    
    [Tooltip("Stamina cool down")]
    [Range(1, 5)]
    public float timeBeforeStaminaRegenStarts = 3f;
    
    [Tooltip("Stamina cool down value")]
    [Range(1, 5)]
    public float staminaIncrement = 2f;
    
    [Tooltip("Time between stamina increment")]
    [Range(0.01f, 1f)]
    public float staminaTimeIncrement = 0.1f;

    [Space(10)]
    [Header("Move")]
    public float walkSpeed;
    public float sprintSpeed;

    [Space(10)]
    [Header("Look")]
    [Range(1, 100)]
    public float baseSensitive;
    
    [Range(45, 90)]
    public float lookLimit;

    [Header("Ground Check")]
    public float groundRadius = 0.4f;
    public LayerMask groundMask;
    public float gravity = -9.81f;
}