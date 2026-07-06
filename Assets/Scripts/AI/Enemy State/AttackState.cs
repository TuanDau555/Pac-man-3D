using UnityEngine;
using UnityEngine.AI;

public class AttackState : EnemyBaseState
{   
    #region Parameters

    private readonly NavMeshAgent _agent;
    private readonly EnemyCombat combat;
    private readonly EnemyFOV _fov;
    
    #endregion
    
    #region Constrcutor
    
    public AttackState(NavMeshAgent agent, Enemy enemy, EnemyCombat combat, EnemyFOV fov) : base(enemy)
    {
        this._agent = agent;
        this.combat = combat;
        this._fov = fov;
    }

    #endregion

    #region Execute

    public override void OnEnter()
    {
        _agent.isStopped = true;
    }

    public override void Update()
    {
        if(_fov.CurrentTarget == null) return;

        var damageable = _fov.CurrentTarget.GetComponent<IDamageable>();
        if(damageable == null) return;
        
        if(combat.CanAttack())
        {
            combat.Attack(damageable);
        }
    }

    public override void OnExit()
    {
        _agent.isStopped = false;
    }
    
    #endregion
}