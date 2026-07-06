using System;
using UnityEngine;
using UnityEngine.AI;

public class ChaseState : EnemyBaseState
{
    #region Parameter
    private NavMeshAgent _navMeshAgent;
    private EnemyStatsSO _statsSO;
    private EnemyFOV _fov;
    private float _viewRadius;
    private float _viewAngle;
    #endregion

    #region Constructor 
    public ChaseState(Enemy enemy, NavMeshAgent agent, EnemyFOV fov, EnemyStatsSO statsSO) : base(enemy)
    {
        this._navMeshAgent = agent;
        this._fov = fov;
        this._statsSO = statsSO;
    }
    #endregion

    #region Execute
    public override void OnEnter()
    {
        // animator.CrossFade(ChaseHash, crossFadeDuration);

        Debug.Log($"{enemy.name} is chasing");
        InitializeAgent();

        _navMeshAgent.SetDestination(_fov.CurrentTarget.position);
    }

    public override void Update()
    {
        ChasePlayer();
    }


    public override void OnExit()
    {
        _navMeshAgent.ResetPath();
    }
    #endregion

    #region Agent
    private void InitializeAgent()
    {
        if (_navMeshAgent == null || _fov == null) return;

        _navMeshAgent.isStopped = false;
        _navMeshAgent.speed = _statsSO.chaseSpeed;
        _navMeshAgent.stoppingDistance = _statsSO.attackDistance;

    }
    #endregion

    #region States
    private void ChasePlayer()
    {
        Vector3 playerPos = _fov.CurrentTarget.position;
        Vector3 playerAvgVelocity = _fov.CurrentTarget.GetComponent<PlayerController>().GetAverageVelocity;
        
        // Estimate player's move and time, which enemy needs to go to player position  
        float timeToPlayer = Vector3.Distance(playerPos, _navMeshAgent.transform.position)
        / _statsSO.chaseSpeed;

        // To avoid Prediction player move to far
        // ìf player goes further, the enemy can only prediction the amount of time (here is movePredictionTime) 
        if (timeToPlayer > _statsSO.movePredictionTime)
        {
            timeToPlayer = _statsSO.movePredictionTime;
        }

        // Prediction the position that player may come
        // This current position plus with Distance that player may go in that time
        /// Summary:
        ///     If the player runs in direction X with an average speed of 5m/s and it takes me 1s to catch up to the same place → then the player has gone another 5m, I should run there.
        Vector3 targetPosition = playerPos + playerAvgVelocity * timeToPlayer;


        Vector3 directionToPlayer = (playerPos - _navMeshAgent.transform.position).normalized;
        Vector3 directionToTarget = (targetPosition - _navMeshAgent.transform.position).normalized;

        float dot = Vector3.Dot(directionToPlayer, directionToTarget); // cosin angle between 2 vector

        // if enemy's prediction direction is too far from current player's direction...
        // To Prevent run infront of player
        if (dot < _statsSO.movePredictionThreshold)
        {
            // ...No need to predict go to player position 
            targetPosition = playerPos;
        }

        _navMeshAgent.SetDestination(targetPosition);

    }
    #endregion
}