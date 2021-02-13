/*****************************************************************************
// File Name :         GameManager.cs
// Author :            Kyle Grenier
// Creation Date :     02/02/2020
//
// Brief Description : Class to manage game state. Delegates other background tasks (UI, etc.) to their respective classes.
*****************************************************************************/
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

[RequireComponent(typeof(UIManager))]
public class GameManager : MonoBehaviour
{
    //Reference to the player's health.
    private Health playerHealth;

    //The UIManager that will handle updating game state UI.
    private UIManager uiManager;

    //Is the game over?
    private static bool gameOver = false;
    public static bool GameOver { get { return gameOver; } }

    void Awake()
    {
        //Subscribing to the OnDeath event, so OnPlayerDeath will run when the player dies.
        playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<Health>();
        playerHealth.OnDeath += OnPlayerDeath;

        //Make sure OnGameWin runs when the level is completed.
        Level.OnLevelComplete += OnGameWin;

        uiManager = GetComponent<UIManager>();

        gameOver = false;
    }

    private void Start()
    {
        if (GameObject.FindGameObjectWithTag("Level") == null)
            Debug.LogWarning("GameManager: Cannot find the Level game object.");
    }

    void OnDestroy()
    {
        playerHealth.OnDeath -= OnPlayerDeath;
        Level.OnLevelComplete -= OnGameWin;
    }

    /// <summary>
    /// What occurs when the player dies.
    /// </summary>
    private void OnPlayerDeath()
    {
        if (gameOver) return;
        gameOver = true;
        //Destroy(playerHealth.gameObject); //Destroy the player game object.
        uiManager.OnGameOver();
        StartCoroutine(LoadSceneAfterTime(4f,SceneManager.GetActiveScene().buildIndex)); //TODO: reload the current scene.
    }

    /// <summary>
    /// What occurs when the game is won.
    /// </summary>
    private void OnGameWin()
    {
        if (gameOver) return;
        gameOver = true;
        print("Game is over!");
        uiManager.OnGameWin();
        StartCoroutine(LoadSceneAfterTime(4f,1));
    }

    /// <summary>
    /// Loads a scene after a given amount of time.
    /// </summary>
    /// <param name="t">The time in seconds.</param>
    /// <param name="index">The build index of the scene to load..</param>
    private IEnumerator LoadSceneAfterTime(float t, int index)
    {
        yield return new WaitForSeconds(t);
        SceneManager.LoadScene(index);
    }

    private void Update()
    {
        //Load the menu on ESC key press.
        if (Input.GetButtonDown("Cancel"))
        {
            SceneManager.LoadScene(1);
        }
    }
}
