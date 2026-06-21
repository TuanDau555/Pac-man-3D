using System;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBrain : MonoBehaviour
{
    #region Parameter

    private StateMachine _stateMachine;
    private Enemy _enemy;
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
        _agent = GetComponent<NavMeshAgent>();
        _enemy = GetComponent<Enemy>();
        // _animator = GetComponent<Animator>();
        _enemyCombat = GetComponent<EnemyCombat>();

        _stateMachine = new StateMachine();
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
        
    }

    #endregion

}
