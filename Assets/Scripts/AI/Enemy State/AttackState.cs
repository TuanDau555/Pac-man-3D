using UnityEngine;
using UnityEngine.AI;

public class AttackState : EnemyBaseState
{   
    #region Parameters

    private NavMeshAgent _agent;
    private EnemyCombat combat;
    private IDamageable _damagealbe;
    
    #endregion
    
    #region Constrcutor
    
    public AttackState(NavMeshAgent agent, Enemy enemy, Animator animator, EnemyCombat combat, IDamageable damageable) : base(enemy, animator)
    {
        this._agent = agent;
        this.combat = combat;
        this._damagealbe = damageable;
    }

    #endregion

    #region Execute

    public override void OnEnter()
    {
        _agent.isStopped = false;
    }

    public override void Update()
    {
        if(combat.CanAttack())
        {
            combat.Attack(_damagealbe);
        }
    }

    public override void OnExit()
    {
        _agent.isStopped = true;
    }
    
    #endregion
}