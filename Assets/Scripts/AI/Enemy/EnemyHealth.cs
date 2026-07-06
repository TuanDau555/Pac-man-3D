using System;
using UnityEngine;

public class EnemyHealth : MonoBehaviour, IDamageable
{
    #region Parameters

    [Header("Stats")]
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private int armor = 0;

    private float _currentHealth;
    private bool _isDead;

    public float CurrentHealth => _currentHealth;
    public float MaxHealth => maxHealth;
    public bool IsDead => _isDead;
    
    // Events
    public event EventHandler<EnemyHealth> OnDeath;
    public event EventHandler<float> OnHealthChanged;

    #endregion

    #region Execute
    private void Awake()
    {
        _currentHealth = maxHealth;
    }

    private void OnDestroy()
    {
       OnDeath = null;
       OnHealthChanged = null;
    }

    #endregion
    
    #region IDamageable
    
    public void TakeDamage(int damage)
    {
        if(_isDead) return;
        if(damage <= 0) return; // I don't want to do calculate when there is nothing to do
        
        int finalDamage = Mathf.Max(0, damage - armor);

        float oldHealth = _currentHealth;
        _currentHealth = Mathf.Max(0, _currentHealth - finalDamage);
        
        // Only fire event when it actually changed
        if(_currentHealth != oldHealth)
        {
            OnHealthChanged?.Invoke(this, _currentHealth);
        }

        if(_currentHealth <= 0)
        {
            Die();
        }
    }

    #endregion

    #region Die

    private void Die()
    {
        Debug.Log($"Enemy {gameObject.name} died");

        OnDeath?.Invoke(this, this);
    }
    
    #endregion

    #region Debug

    [ContextMenu("Debug: Take 10 Damage")]
    private void DebugTakeDame() => TakeDamage(10);
    
    #endregion
}