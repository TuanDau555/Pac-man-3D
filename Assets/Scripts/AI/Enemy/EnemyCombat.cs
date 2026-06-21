using UnityEngine;

public class EnemyCombat : MonoBehaviour
{
     #region Parameters
    
    [Header("Stats")]
    [SerializeField] private int damage = 1;
    [SerializeField] private float attackCoolDown = 1.5f;

    private CountdownTimer _attackCooldownTimer;

    #endregion

    #region Excute

    private void Awake()
    {
        _attackCooldownTimer = new CountdownTimer(attackCoolDown);
    }

    private void Update()
    {
        _attackCooldownTimer.Tick(Time.deltaTime);
    }

    #endregion

    #region Attack

    public bool CanAttack()
    {
        return !_attackCooldownTimer.IsRunning;
    }

    public void Attack(IDamageable target)
    {
        if(!CanAttack() || target == null)
            return;
        
        _attackCooldownTimer.Reset(attackCoolDown);
        _attackCooldownTimer.Start();
        
       target.TakeDamage(damage);
    }
    #endregion
}
