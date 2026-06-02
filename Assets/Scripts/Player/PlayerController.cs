using System;
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
}
