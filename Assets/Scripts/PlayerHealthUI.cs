/*****************************************************************************
// File Name :         PlayerHealthUI.cs
// Author :            Kyle Grenier
// Creation Date :     #CREATIONDATE#
//
// Brief Description : ADD BRIEF DESCRIPTION OF THE FILE HERE
*****************************************************************************/
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class PlayerHealthUI : MonoBehaviour, IHealthUI
{
    private int maxHealth = 0;
    [SerializeField] private Sprite fullHeartSprite;
    [SerializeField] private Sprite emptyHeartSprite;

    private Image initialHeart;

    private List<Image> heartList;
    private int lastFullIndex = 0; //The index of the last full (non-empty) heart.



    private void Awake()
    {
        initialHeart = transform.GetChild(0).gameObject.GetComponent<Image>();

        //Subscribe to player's Health events.
        Health playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<Health>();
        playerHealth.OnTakeDamage += DecreaseHealth;
        playerHealth.OnHeal += IncreaseHealth;
    }




    /// <summary>
    /// UI update on health decrease.
    /// </summary>
    /// <param name="amnt">The amount of health that was decreased.</param>
    public void DecreaseHealth(int amnt)
    {
        for (int i = 0; i < amnt; ++i)
        {
            if (lastFullIndex < 0)
            {
                Debug.LogWarning("Player Health UI is trying to decrease a non-existant heart.");
                return;
            }

            UpdateSprite(heartList[lastFullIndex], emptyHeartSprite);
            --lastFullIndex;
        }
    }

    /// <summary>
    /// UI update on health increase.
    /// </summary>
    /// <param name="amnt">The amount of health that was increased.</param>
    public void IncreaseHealth(int amnt)
    {
        if (maxHealth <= 0)
            SetupUI(amnt);
        else
        {
            for (int i = 0; i < amnt; ++i)
            {
                if (lastFullIndex >= maxHealth - 1)
                {
                    Debug.LogWarning("Player Health UI is trying to add additional health.");
                    return;
                }
                else
                {
                    UpdateSprite(heartList[lastFullIndex + 1], fullHeartSprite);
                    ++lastFullIndex;
                }
            }
        }
    }

    /// <summary>
    /// Sets up the UI to have all necessary hearts.
    /// </summary>
    /// <param name="amnt">The max health; the number of hearts to display on screen.</param>
    private void SetupUI(int amnt)
    {
        if (amnt <= 0)
        {
            Debug.LogWarning("Health UI trying to setup with a health of 0!");
            return;
        }

        maxHealth = amnt;
        UpdateSprite(initialHeart, fullHeartSprite);

        //Initialize the list with a capacity of maxHealth;
        heartList = new List<Image>(maxHealth);
        heartList.Add(initialHeart);
        lastFullIndex = maxHealth - 1;

        for (int i = 0; i < maxHealth - 1; ++i)
        {
            //Clones the initial heart.
            heartList.Add(Instantiate(initialHeart, transform).GetComponent<Image>());
        }

    }

    /// <summary>
    /// Assigns an Image a new Sprite.
    /// </summary>
    /// <param name="obj">The Image component to assign a sprite to.</param>
    /// <param name="sprite">The sprite to assign to the Image component.</param>
    private void UpdateSprite(Image img, Sprite sprite)
    {
        img.sprite = sprite;
    }
}
