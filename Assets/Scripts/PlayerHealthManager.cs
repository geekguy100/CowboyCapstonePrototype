/*****************************************************************************
// File Name :         PlayerHealthManager.cs
// Author :            Kyle Grenier
// Creation Date :     02/02/2020
//
// Brief Description : Subscribes the OnPlayerDeath method to the OnDeath event in the player's Health class.
//                     Helps manage decoupled code. I thought it would be better to put this in here instead of 
                       having the GameManager find the player's Health script and subscribe itself.
*****************************************************************************/
using UnityEngine;

[RequireComponent(typeof(Health))]
public class PlayerHealthManager : MonoBehaviour
{
    private Health playerHealth;

    private void Awake()
    {
        playerHealth = GetComponent<Health>();
        playerHealth.OnDeath += GameManager.Instance.OnPlayerDeath;
    }

    private void OnDestroy()
    {
        playerHealth.OnDeath -= GameManager.Instance.OnPlayerDeath;
    }
}
