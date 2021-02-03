/*****************************************************************************
// File Name :         Health.cs
// Author :            Kyle Grenier
// Creation Date :     02/02/2020
//
// Brief Description : Health class for anything that needs to have health.
*****************************************************************************/
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField]
    private float maxHealth = 100f;
    private float currentHealth;

    public delegate void OnDeathHandler();
    public event OnDeathHandler OnDeath;

    private void Awake()
    {
        currentHealth = maxHealth;
    }

    /// <summary>
    /// Decreases the amount of current health.
    /// </summary>
    /// <param name="damage">The amount to decrease the current health by.</param>
    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
            Die();
    }

    /// <summary>
    /// Adds more to the current health.
    /// </summary>
    /// <param name="amnt">The amount of health to add.</param>
    public void AddHealth(float amnt)
    {
        if (currentHealth + amnt > maxHealth)
            currentHealth = maxHealth;
        else
            currentHealth += amnt;
    }

    /// <summary>
    /// Run the OnDeath event if anything is subscribed to it.
    /// </summary>
    private void Die()
    {
        OnDeath?.Invoke();
    }

}
