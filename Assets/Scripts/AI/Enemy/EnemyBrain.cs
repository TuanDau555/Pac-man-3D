using System;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBrain : MonoBehaviour
{
    #region Parameter

    [Tooltip("Area that Enemy Patrol")]
    [SerializeField] private Transform _patrolRegion;
    [SerializeField] private EnemyStatsSO _enemyStatsSO;

    private StateMachine _stateMachine;
    private Enemy _enemy;
    private EnemyFOV _fov;
    private EnemyCombat _enemyCombat;
    private NavMeshAgent _agent;

    #endregion

    #region Execute

    private void Awake()
    {
        Init();
    }

    private void Update()
    {
        if (_stateMachine == null)
        {
            Debug.LogError($"EnemyBrain on {gameObject.name} has not been initialized.");
            Debug.Log($"State machine: {_stateMachine}");
            return;
        }

        _stateMachine.Update();
    }

    private void FixedUpdate()
    {
        if (_stateMachine == null)
        {
            Debug.LogError($"EnemyBrain on {gameObject.name} has not been initialized.");
            Debug.Log($"State machine: {_stateMachine}");
            return;
        }

        _stateMachine.FixedUpdate();
    }

    #endregion

    #region Init


    private void Init()
    {
        _stateMachine = new StateMachine();
        _agent = GetComponent<NavMeshAgent>();
        _enemy = GetComponent<Enemy>();
        _fov = GetComponent<EnemyFOV>();
        // _animator = GetComponent<Animator>();
        _enemyCombat = GetComponent<EnemyCombat>();

        EnemyState(_agent, _enemy, _enemyCombat);

    }

    #endregion

    #region State Change

    private void At(IState from, IState to, IPredicate condition)
        => _stateMachine.AddTransition(from, to, condition);

    private void Any(IState to, IPredicate condition)
        => _stateMachine.AddAnyTransition(to, condition);

    #endregion

    #region State

    private void EnemyState(NavMeshAgent agent, Enemy enemy, EnemyCombat enemyCombat)
    {
        var patrolState = new PatrolState(enemy, agent, _patrolRegion, _enemyStatsSO);
        var chaseState = new ChaseState(enemy, agent, _fov, _enemyStatsSO);
        var attackState = new AttackState(agent, enemy, enemyCombat, _fov);

        At(patrolState, chaseState, new FuncPredicate(() => _fov.CanSeePlayer && !_fov.InAttackRange));
        At(chaseState, attackState, new FuncPredicate(() => _fov.InAttackRange));
        At(attackState, chaseState, new FuncPredicate(() => !_fov.InAttackRange && _fov.CanSeePlayer));
        At(chaseState, patrolState, new FuncPredicate(() => !_fov.CanSeePlayer));

        _stateMachine.SetState(patrolState);
    }

    #endregion

}
