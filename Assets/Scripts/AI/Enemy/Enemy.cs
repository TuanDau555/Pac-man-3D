using UnityEngine;

[RequireComponent(typeof(EnemyHealth))]
public class Enemy : MonoBehaviour
{
    #region Parameter

    private EnemyHealth _health;

    public EnemyHealth Health => _health;

    #endregion

    #region Execute

    private void Awake()
    {
        _health = GetComponent<EnemyHealth>();
    }

    private void OnEnable()
    {
        _health.OnDeath += HandleDeath;
    }

    private void OnDisable()
    {
        _health.OnDeath -= HandleDeath;
    }

    #endregion

    #region Events

    private void HandleDeath(object sender, EnemyHealth e)
    {
        Debug.Log($"Enemy {gameObject.name} died");
    }
    
    #endregion
}
