/*****************************************************************************
// File Name :         UIManager.cs
// Author :            Kyle Grenier
// Creation Date :     02/10/2021
//
// Brief Description : Handles updating game state UI.
*****************************************************************************/
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    [Tooltip("The UI GameObject that holds all of the other UI GameObjects to be activated on game end.")]
    [SerializeField] private GameObject gameEndParent;

    [Tooltip("The text used to inform the player of the game's state.")]
    [SerializeField] private TextMeshProUGUI gameStateText;

    private void Start()
    {
        gameEndParent.SetActive(false);
    }

    public void OnGameOver()
    {
        gameEndParent.SetActive(true);
        gameStateText.text = "GAME OVER!";
    }

    public void OnGameWin()
    {
        gameEndParent.SetActive(true);
        gameStateText.text = "WINNER!";
    }
}
