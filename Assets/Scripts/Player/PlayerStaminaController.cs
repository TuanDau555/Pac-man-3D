using System;
using UnityEngine;

[RequireComponent(typeof(PlayerController))]
public class PlayerStaminaController : MonoBehaviour
{
    #region Paramters

    [Header("Required Component")]
    [SerializeField] private PlayerStatsSO _playerStats;

    private PlayerController _playerController;

    private CountdownTimer _regenDelayTimer;
    private CountdownTimer _regenTickTimer;

    private float _currentStamina;
    private float _maxStamina;
    private float _staminaIncrementValue;

    #endregion

    #region Excute

    private void Awake()
    {
        InitStats(_playerStats);

        _regenDelayTimer = new CountdownTimer(0);
        _regenTickTimer = new CountdownTimer(0);

        // regen stamin after delay time
        _regenDelayTimer.OnTimerStop += RegenDelay_OnTimerStop;

        // Increase stamina until it full
        _regenTickTimer.OnTimerStop += RegenTick_OnTimerStop;
    }

    private void Start()
    {
        _playerController = GetComponent<PlayerController>();
    }

    private void Update()
    {
        _regenDelayTimer.Tick(Time.deltaTime);
        _regenTickTimer.Tick(Time.deltaTime);

        HandleStamina(
            _maxStamina,
            _playerStats.staminaUseMultiplier,
            _playerStats.timeBeforeStaminaRegenStarts,
            _playerStats.staminaTimeIncrement,
            _staminaIncrementValue);
    }

    #endregion

    #region Init

    private void InitStats(PlayerStatsSO stats)
    {
        if (_playerStats == null) return;

        _maxStamina = stats.stamina;
        _currentStamina = _maxStamina;
        _staminaIncrementValue = stats.staminaIncrement;
    }

    #endregion

    #region Events

    private void RegenDelay_OnTimerStop()
    {
        if (_currentStamina < _maxStamina)
        {
            _playerController.CanSprint = true;
            _regenTickTimer.Start();
        }
        else
        {
            _playerController.CanSprint = true;
        }
    }

    private void RegenTick_OnTimerStop()
    {
        _currentStamina = Mathf.Min(_currentStamina + _staminaIncrementValue, _maxStamina);

        if (_currentStamina < _maxStamina)
        {
            _regenTickTimer.Start();
        }
        else
        {
            _regenTickTimer.Stop();
        }
    }

    #endregion

    #region Stamina Calcualate

    /// <summary>
    /// Reduce player stamina when they are sprint
    /// </summary>
    /// <param name="maxStamina">The highest stamina amout that player have</param>
    /// <param name="amout">Stamina use value</param>
    /// <param name="startRegen">Stamina cool down</param>
    /// <param name="timeIncrement">Time beween each increment</param>
    /// <param name="incrementValue">Stamina increase value over time</param>
    private void HandleStamina(float maxStamina, float amout, float startRegen, float timeIncrement, float incrementValue)
    {
        _maxStamina = maxStamina;
        _staminaIncrementValue = incrementValue;

        bool isMoving = _playerController.MoveInput.magnitude > 0.1f;

        if (InputManager.Instance.IsSprintedPressed() && isMoving && _playerController.CanSprint)
        {

            // We don't want to regen it while the player are sprinting
            _regenDelayTimer.Stop();
            _regenTickTimer.Stop();

            _currentStamina -= amout * Time.deltaTime;
            _currentStamina = Mathf.Max(_currentStamina, 0);

            if (_currentStamina <= 0f)
            {
                _playerController.CanSprint = false;
            }
        }
        else if (!InputManager.Instance.IsSprintedPressed() && _currentStamina < maxStamina)
        {
            // Start regen if player stop sprinting
            if (!_regenDelayTimer.IsRunning && !_regenTickTimer.IsRunning)
            {
                _regenDelayTimer.Reset(startRegen);
                _regenDelayTimer.Start();
            }
        }
    }

    #endregion
}
