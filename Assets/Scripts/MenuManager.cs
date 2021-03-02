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
    public GameObject tutorialPanel2;
    public GameObject tutorialPanel3;
    public GameObject tutorialPanel4;
    public GameObject creditsPanel;

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
        tutorialPanel2.SetActive(false);
        tutorialPanel3.SetActive(false);
        tutorialPanel4.SetActive(false);
        creditsPanel.SetActive(false);
        mainPanel.SetActive(true);
    }

    public void DisplayTutorialPanel2()
    {
        tutorialPanel1.SetActive(false);
        mainPanel.SetActive(false);
        tutorialPanel2.SetActive(true);
    }

    public void DisplayTutorialPanel3()
    {
        tutorialPanel2.SetActive(false);
        mainPanel.SetActive(false);
        tutorialPanel3.SetActive(true);
    }

    public void DisplayTutorialPanel4()
    {
        tutorialPanel3.SetActive(false);
        mainPanel.SetActive(false);
        tutorialPanel4.SetActive(true);
    }

    public void OpenCreditsPanel()
    {
        creditsPanel.SetActive(true);
        mainPanel.SetActive(false);
    }

    public void CloseCreditsPanel()
    {
        creditsPanel.SetActive(false);
        mainPanel.SetActive(true);
    }
}
