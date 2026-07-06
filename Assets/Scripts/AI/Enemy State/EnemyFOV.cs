using UnityEditor;
using UnityEngine;

public class EnemyFOV : MonoBehaviour
{
    #region Parameters

    [Header("Required")]
    [SerializeField] private EnemyStatsSO enemyStatsSO;
    [SerializeField] private float fovCheckInterval = 0.2f;
    [SerializeField] private float playerSearchInterval = 0.2f;

    private float _viewRadius;
    private float _viewAngle;
    private float _attackRange;

    [ReadOnly] private Collider[] _rangeChecks;
    [ReadOnly] private Transform _playerRef;

    // Public state
    public bool CanSeePlayer { get; private set; }
    public bool InAttackRange { get; private set; }
    public Vector3 DirectionToTarget { get; private set; }
    public Transform CurrentTarget { get; private set; }

    // Timers
    private CountdownTimer _fovTickTimer;
    private CountdownTimer _playerSearchTimer;
    private CountdownTimer _lostSightTimer;

    #endregion

    #region Excute

    private void Awake()
    {
        InitStats();
        InitTimers();
    }

    private void Update()
    {
        _fovTickTimer.Tick(Time.deltaTime);
        _playerSearchTimer.Tick(Time.deltaTime);
        _lostSightTimer.Tick(Time.deltaTime);
    }

    #endregion

    #region Init

    private void InitStats()
    {
        if (enemyStatsSO == null)
        {
            Debug.LogWarning($"[FieldOfView] EnemyStatsSO is null on {gameObject.name}");
            return;
        }

        _viewRadius = enemyStatsSO.radius;
        _viewAngle = enemyStatsSO.angle;
        _attackRange = enemyStatsSO.attackDistance;
    }

    private void InitTimers()
    {
        // periodic FOV tick
        _fovTickTimer = new CountdownTimer(fovCheckInterval);
        _fovTickTimer.OnTimerStop += () =>
        {
            FieldOfViewCheck();
            _fovTickTimer.Start(); // loop
        };
        _fovTickTimer.Start();

        _playerSearchTimer = new CountdownTimer(playerSearchInterval);
        _playerSearchTimer.OnTimerStop += () =>
        {
            if (_playerRef == null)
            {
                _playerRef = FindAnyObjectByType<PlayerController>()?.transform;
                _playerSearchTimer.Start();
            }
            // If we already found the player dont start again, stop it
        };
        _playerSearchTimer.Start();

        // lost sight timer
        // Only start when lost sight
        _lostSightTimer = new CountdownTimer(enemyStatsSO.lostSightDelay);
        _lostSightTimer.OnTimerStop += () =>
        {
            if (CanSeePlayer)
            {
                CanSeePlayer = false;
                Debug.Log($"[FOV] {_playerRef?.name} lost by {gameObject.name}");
            }
        };
    }

    #endregion

    #region FOV Check

    private void FieldOfViewCheck()
    {
        if (_playerRef == null) return;

        // Reset flag this tick; will be set true again if we find valid sight
        bool sawTarget = false;
        InAttackRange = false;

        // Get all targets in view radius (It just player by the way XD)
        _rangeChecks = Physics.OverlapSphere(
            transform.position,
            _viewRadius,
            enemyStatsSO.targetMask
        );

        // if found something in the range
        if (_rangeChecks.Length > 0)
        {
            Transform target = _rangeChecks[0].transform;
            CurrentTarget = target;
            DirectionToTarget = (target.position - transform.position).normalized;
            float distanceToTarget = Vector3.Distance(transform.position, target.position);

            bool isBlocked = Physics.Raycast(
                transform.position,
                DirectionToTarget,
                distanceToTarget,
                enemyStatsSO.obstructionMask
            );

            if (!isBlocked)
            {
                // Attack range check
                if (distanceToTarget <= _attackRange)
                {
                    InAttackRange = true;
                    CanSeePlayer = true;
                    sawTarget = true;
                }

                // Angle check
                float angleToTarget = Vector3.Angle(transform.forward, DirectionToTarget);
                if (angleToTarget < _viewAngle / 2f)
                {
                    sawTarget = true;

                    if (!CanSeePlayer)
                    {
                        CanSeePlayer = true;
                        Debug.Log($"[FOV] {gameObject.name} gained sight of {_playerRef.name}");
                    }
                }
            }
        }
        else
        {
            CurrentTarget = null;
        }

        if (sawTarget)
        {
            _lostSightTimer.Stop();
            _lostSightTimer.Reset();
        }
        else if (!_lostSightTimer.IsRunning && CanSeePlayer)
        {
            // Start coundown when lost sight
            _lostSightTimer.Start();
        }
    }

    #endregion

    #region Editor
    private void OnDrawGizmos()
    {
        Handles.color = Color.yellow;
        Handles.DrawWireArc(transform.position, Vector3.up, Vector3.forward, 360, _viewRadius);
        Handles.DrawWireArc(transform.position, Vector3.up, Vector3.forward, 360, _attackRange);

        Vector3 viewAngleA = DirFromAngle(transform.eulerAngles.y, -_viewAngle / 2);
        Vector3 viewAngleB = DirFromAngle(transform.eulerAngles.y, _viewAngle / 2);

        Handles.DrawLine(transform.position, transform.position + viewAngleA * _viewRadius);
        Handles.DrawLine(transform.position, transform.position + viewAngleB * _viewRadius);

        if (CanSeePlayer)
        {
            Handles.color = Color.green;
            Handles.DrawLine(transform.position, _playerRef.transform.position);
        }
    }

    private Vector3 DirFromAngle(float eulerY, float angleInDegrees)
    {
        angleInDegrees += eulerY;
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }
    #endregion
}