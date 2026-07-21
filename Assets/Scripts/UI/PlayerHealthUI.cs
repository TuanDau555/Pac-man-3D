using System;
using TMPro;
using UnityEngine;

public class PlayerHealthUI : MonoBehaviour
{
    #region Parameters

    [Header("Ref")]
    [SerializeField] private PlayerHealthController playerHealth;
    [SerializeField] private TextMeshProUGUI healthText;

    #endregion

    #region Unity Lifecycle

    private void Awake()
    {
        if (playerHealth == null)
        {
            Debug.LogWarning("[PlayerHealthUI] Player Health Controller is null");
            return;
        }

    }

    private void OnEnable()
    {
        if (playerHealth == null) return;

        Debug.Log(playerHealth.CurrentHealth);

        playerHealth.OnHealthChanged += HandleHealthChanged;
        UpdateHealthText(playerHealth.CurrentHealth);
    }

    private void OnDisable()
    {
        if (playerHealth == null) return;

        playerHealth.OnHealthChanged -= HandleHealthChanged;

    }

    #endregion

    #region Events

    private void HandleHealthChanged(object sender, int currentHealth)
    {
        UpdateHealthText(currentHealth);
    }

    #endregion

    #region Update Health

    private void UpdateHealthText(int health)
    {
        healthText.text = $"x{health}";
    }

    #endregion
}