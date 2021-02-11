/*****************************************************************************
// File Name :         MenuManager.cs
// Author :            Kyle Grenier
// Creation Date :     02/10/2021
//
// Brief Description : Manages what pressing buttons on the menu does.
*****************************************************************************/
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    /// <summary>
    /// Loads the scene based on the scene's build index.
    /// </summary>
    /// <param name="index">The scene's build index.</param>
    public void LoadScene(int index)
    {
        SceneManager.LoadScene(index);
    }

    /// <summary>
    /// Exits the application.
    /// </summary>
    public void QuitGame()
    {
        Application.Quit();
    }
}
