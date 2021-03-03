/*****************************************************************************
// File Name :         Health.cs
// Author :            Kyle Grenier
// Creation Date :     02/02/2020
//
// Brief Description : Health class for anything that needs to have health.
*****************************************************************************/
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    [SerializeField]
    private int maxHealth = 4;
    private int currentHealth;

    public delegate void OnDeathHandler();
    public event OnDeathHandler OnDeath;

    public delegate void OnTakeDamageHandler(int damage);
    public event OnTakeDamageHandler OnTakeDamage;

    public delegate void OnHealHandler(int currentHealth);
    public event OnHealHandler OnHeal;

    [Tooltip("The amount of time after taking damage that must be wait before " +
        "being able to take damage again.")]
    [SerializeField] private float healthBufferTime;
    private float currentHealthTime = 0f;

    AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        currentHealth = maxHealth;
        currentHealthTime = healthBufferTime;
    }

    private void Start()
    {

        //Make sure to update any UI that is keeping track of our health.
        OnHeal?.Invoke(currentHealth);
    }

    private void Update()
    {
        if (currentHealthTime < healthBufferTime)
            currentHealthTime += Time.deltaTime;
    }

    /// <summary>
    /// Decreases the amount of current health.
    /// </summary>
    /// <param name="damage">The amount to decrease the current health by.</param>
    public void TakeDamage(int damage)
    {
        // Can only take damage if we've waited long enough.
        if (currentHealthTime >= healthBufferTime)
        {
            if (audioSource != null)
                audioSource.Play();

            currentHealthTime = 0;
            //Hit.Play();
            currentHealth -= damage;
            OnTakeDamage?.Invoke(damage);
            if (currentHealth <= 0)
                Die();
        }

    }

    /// <summary>
    /// Adds more to the current health.
    /// </summary>
    /// <param name="amnt">The amount of health to add.</param>
    public void AddHealth(int amnt)
    {
        if (currentHealth + amnt > maxHealth)
        {
            OnHeal?.Invoke(maxHealth - currentHealth);
            currentHealth = maxHealth;
        }
        else
        {
            currentHealth += amnt;
            OnHeal?.Invoke(amnt);
        }
    }

    /// <summary>
    /// Run the OnDeath event if anything is subscribed to it.
    /// </summary>
    private void Die()
    {
        if (!GameManager.GameOver)
        {
            OnDeath?.Invoke();
            Destroy(gameObject);
        }
    }

}
