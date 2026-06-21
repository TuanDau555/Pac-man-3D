using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    #region Parameters

    [Header("Required Component")]
    [SerializeField] private PlayerStatsSO playerStatsSO;
    [SerializeField] private Transform playerCam;

    [Header("Ground Check")]
    [SerializeField] private Transform groundCheckSphere;

    private CharacterController _playerController;
    private InputManager _inputManager;

    // Historical Posision
    [SerializeField]
    [Range(0.001f, 1f)]
    // The time between update player's latest position  
    private float historicalPositionInterval = 0.1f;

    private float lastPositionTime;
    private int maxQueueSize;

    // Store all position in a Queue
    private Queue<Vector3> historicalVelocities;

    private Vector3 averageVelocity;


    private float _clampAngle;
    private float _mouseSen;

    private float _walkSpeed;
    private float _sprintSpeed;
    private float _currentSpeed;

    private Vector3 _playerVelocity;
    private Vector3 _moveDirection;
    private Vector3 _mouseDirection;
    private Vector2 _mouseDelta;

    public Vector2 MoveInput { get; private set; }
    public bool CanSprint { get; set; } = true;

    /// <summary>
    /// Predict the next player Pos
    /// </summary>
    public Vector3 GetAverageVelocity => AverageVelocity();

    #endregion

    #region Excute

    private void Awake()
    {
        if (_playerController == null)
            _playerController = GetComponent<CharacterController>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Start()
    {

        if (_mouseDirection == null)
        {
            _mouseDirection = transform.localRotation.eulerAngles;
        }

        _inputManager = InputManager.Instance;
        playerCam = Camera.main.transform;

        InitStat(playerStatsSO);

    }

    private void Update()
    {
        HandleMovementInput();
        HandleLookInput();

        ProcessMove();

        UpdateHistoricalPosition();

    }

    #endregion

    #region Init Stats

    private void InitStat(PlayerStatsSO stats)
    {
        // Mouse Sen
        _mouseSen = stats.baseSensitive;
        _clampAngle = stats.lookLimit;

        // Move
        _walkSpeed = stats.walkSpeed;
        _sprintSpeed = stats.sprintSpeed;
    }

    #endregion

    #region Input

    private void HandleMovementInput()
    {
        // Get current Input
        MoveInput = _inputManager.GetPlayerMovement();

        // Convert into movement direction
        _moveDirection = new Vector3(MoveInput.x, 0, MoveInput.y).normalized;
    }

    private void HandleLookInput()
    {
        _mouseDelta = _inputManager.GetMouseDelta();
        ApplyFinalLook(_clampAngle);
    }

    #endregion

    #region Movement Calculate

    private void ProcessMove()
    {
        if (IsGround() && _playerVelocity.y < 0)
        {
            // reset Y axis velocity
            _playerVelocity.y = -2f;
        }

        // Apply gravity
        _playerVelocity.y += playerStatsSO.gravity * Time.deltaTime;

        if (_inputManager.IsSprintedPressed() && CanSprint)
        {
            _currentSpeed = _sprintSpeed;
        }
        else
        {
            _currentSpeed = _walkSpeed;
        }

        // Apply movement
        Vector3 finalMovement = (_moveDirection * _currentSpeed) + (_playerVelocity.y * Vector3.up);
        _playerController.Move(finalMovement * Time.deltaTime);
    }

    private void ApplyFinalLook(float clampAngle)
    {
        // Look sen
        _mouseDirection.x += _mouseDelta.x * _mouseSen * Time.deltaTime;
        _mouseDirection.y += _mouseDelta.y * _mouseSen * Time.deltaTime;

        // Look angle
        _mouseDirection.y = Mathf.Clamp(_mouseDirection.y, -clampAngle, clampAngle);

        // Rotation
        transform.localRotation = Quaternion.Euler(-_mouseDirection.y, _mouseDirection.x, 0f);

        // Follow the camera look
        _moveDirection = playerCam.TransformDirection(_moveDirection);
    }

    private bool IsGround()
        => Physics.CheckSphere(groundCheckSphere.position, playerStatsSO.groundRadius, playerStatsSO.groundMask);

    #endregion

    #region Historical Pos

    private void UpdateHistoricalPosition()
    {
        // Only add player's velocities every certain amount of time to avoid updating too frequent  
        if (lastPositionTime + historicalPositionInterval <= Time.time)
        {
            // if queue is ful of player's velocities...
            if (historicalVelocities.Count == maxQueueSize)
            {
                //... Delete old one
                historicalVelocities.Dequeue();
            }

            //... And add new one
            historicalVelocities.Enqueue(_playerController.velocity);

            lastPositionTime = Time.time;
        }
    }

    /// <summary>
    /// Calculates the average horizontal (XZ) velocity from the recorded historicalVelocities.
    /// Ignores the vertical (Y) component and returns Vector3.zero if there are no samples.
    /// </summary>
    /// <returns>Average horizontal velocity as a Vector3 (Y = 0).</returns>
    private Vector3 AverageVelocity()
    {
        // Prevent null and division by 0
        if (historicalVelocities == null || historicalVelocities.Count == 0)
            return Vector3.zero;

        averageVelocity = Vector3.zero;
        foreach (Vector3 velocity in historicalVelocities)
        {
            averageVelocity += velocity;
        }
        averageVelocity.y = 0;
        return averageVelocity / historicalVelocities.Count;
    }

    #endregion
}
