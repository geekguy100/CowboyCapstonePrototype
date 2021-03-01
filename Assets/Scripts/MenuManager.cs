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
    public GameObject mainPanel;
    public GameObject tutorialPanel1;

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

    //Should adjust master volume
    public void AdjustVolume(float newVolume)
    {
        AudioListener.volume = newVolume;
    }

    public void DisplayTutorialPanel1()
    {
        tutorialPanel1.SetActive(true);
        mainPanel.SetActive(false);
    }

    public void ReturnToMenu()
    {
        tutorialPanel1.SetActive(false);
        mainPanel.SetActive(true);
    }
}
