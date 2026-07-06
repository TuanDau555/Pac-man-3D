using System;
using UnityEngine;

public class PlayerHealthController : MonoBehaviour, IDamageable
{
    #region Parameters

    [Header("Required Component")]
    [SerializeField] private PlayerStatsSO _playerStats;

    private int _maxHealth;
    
    [ReadOnly, SerializeField] 
    private int _currentHealth;

    public bool IsDead => _currentHealth <= 0;

    public int CurrentHealth => _currentHealth;
    public int MaxHealth => _maxHealth;
    
    public event EventHandler OnDeath;
    public event EventHandler<int> OnHealthChanged;

    #endregion

    #region Execute

    private void Awake()
    {
        InitStats(_playerStats);
    }

    private void OnDisable()
    {
        OnDeath = null;
        OnHealthChanged = null;
    }
    
    #endregion

    #region Init
    
    private void InitStats(PlayerStatsSO stats)
    {
        if(_playerStats == null) return;

        _maxHealth = stats.HP;
        _currentHealth = _maxHealth;
    }

    #endregion
    
    #region IDamageable

    [ContextMenu("Take Damage")]
    public void TakeDamage(int damage)
    {
        if(IsDead || damage == 0) return;

        int oldHealth = _currentHealth;

        _currentHealth = Mathf.Max(0, _currentHealth - damage);

        HandleHealthChanged(oldHealth, _currentHealth);
    }
    
    #endregion

    #region Health

    public void Heal(int amount)
    {
        if(IsDead || amount <= 0) return;

        int oldHealth = _currentHealth;
        _currentHealth = Mathf.Min(_maxHealth, _currentHealth + amount);
        
        HandleHealthChanged(oldHealth, _currentHealth);
    }

    /// <summary>
    /// Heal full heal immediately
    /// </summary>
    /// <remarks>Use for respawn or checkpoint</remarks>
    public void FullHeal()
    {
        int oldHealth = _currentHealth;
        _currentHealth = _maxHealth;

        HandleHealthChanged(oldHealth, _currentHealth);
    }
    
    private void HandleHealthChanged(int oldValue, int newValue)
    {
        if(oldValue == newValue) return;
        
        OnHealthChanged?.Invoke(this, newValue);

        if(newValue <= 0 && oldValue > 0)
        {
            OnDeath?.Invoke(this, EventArgs.Empty);
        }
    }
    
    #endregion

    #region Debug

    [ContextMenu("Debug: Take 1 damage")]
    private void DebugTakeDamage() => TakeDamage(1);
    
    [ContextMenu("Debug: Heal 1")]
    private void DebugHeal() => Heal(1);
    
    [ContextMenu("Debug: Full Heal")]
    private void DebugFullHeal() => FullHeal();
    
    #endregion
}
