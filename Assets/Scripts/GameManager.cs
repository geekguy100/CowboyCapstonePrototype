/*****************************************************************************
// File Name :         GameManager.cs
// Author :            Kyle Grenier
// Creation Date :     02/02/2020
//
// Brief Description : Class to manage game state.
*****************************************************************************/
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //Reference to the player's health.
    private Health playerHealth;

    void Awake()
    {
        //Subscribing to the OnDeath event, so OnPlayerDeath will run when the player dies.
        playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<Health>();
        playerHealth.OnDeath += OnPlayerDeath;
    }

    void OnDestroy()
    {
        playerHealth.OnDeath -= OnPlayerDeath;
    }

    /// <summary>
    /// What occurs when the player dies.
    /// </summary>
    private void OnPlayerDeath()
    {
        print("The player died oh no!!");
    }
}
