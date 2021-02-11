/*****************************************************************************
// File Name :         Level.cs
// Author :            Kyle Grenier
// Creation Date :     02/10/2021
//
// Brief Description : Handles the finer details of a level such as how many enemies need to be killed, how many enemies have been killed, etc.
*****************************************************************************/
using UnityEngine;

public class Level : MonoBehaviour
{
    //requiredEnemyKills updates according to the number of enemies in the scene.
    private int requiredEnemyKills = 0;
    private int enemiesKilled = 0;

    public delegate void LevelCompleteHandler();
    public static event LevelCompleteHandler OnLevelComplete;



    /// <summary>
    /// Subscribes to the enemies' Health.OnDeath events and increase requiredEnemyKills 
    /// to the number of enemies in the scene.
    /// </summary>
    private void Start()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies)
        {
            enemy.GetComponent<Health>().OnDeath += KilledEnemy;
            ++requiredEnemyKills;
        }
    }

    /// <summary>
    /// Unsubscribes to the enemies' Health.OnDeath events.
    /// </summary>
    private void OnDisable()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies)
        {
            enemy.GetComponent<Health>().OnDeath -= KilledEnemy;
        }
    }

    /// <summary>
    /// Increases score on enemy killed.
    /// </summary>
    private void KilledEnemy()
    {
        ++enemiesKilled;
        print("LEVEL: An enemy died, totaling " + enemiesKilled + " enemies. " + (requiredEnemyKills - enemiesKilled) + " enemies remain.");
        if (enemiesKilled >= requiredEnemyKills)
            OnLevelComplete?.Invoke();
    }
}
