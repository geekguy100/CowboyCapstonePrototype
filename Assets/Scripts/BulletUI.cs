using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;


public class BulletUI : MonoBehaviour
{
    int maxBullets = 0;
    [SerializeField] private Sprite fullBulletSprite;
    [SerializeField] private Sprite emptyBulletSprite;

    private Image initialBullet;

    private List<Image> bulletList;
    private int lastFullIndex = 0; //The index of the last full (non-empty) bullet.

    private Weapon playerWeapon;



    private void Awake()
    {
        initialBullet = transform.GetChild(0).gameObject.GetComponent<Image>();

        playerWeapon = GameObject.Find("Player").GetComponentInChildren<Weapon>();

        if (playerWeapon == null)
        {
            Debug.LogWarning(gameObject.name + ": Cannot update bullet UI bc there is no player weapon!");
            Destroy(gameObject);
            return;
        }

        playerWeapon.OnWeaponFire += DecreaseBullets;
        playerWeapon.OnReloadComplete += () => { IncreaseBullets(playerWeapon.GetClipSize()); };
    }

    private void OnDisable()
    {
        if (playerWeapon != null)
            playerWeapon.OnWeaponFire -= DecreaseBullets;
    }

    private void Start()
    {
        IncreaseBullets(playerWeapon.GetMagSize());
    }



    /// <summary>
    /// UI update on Bullets decrease.
    /// </summary>
    /// <param name="amnt">The amount of Bullets that was decreased.</param>
    public void DecreaseBullets(int amnt)
    {
        for (int i = 0; i < amnt; ++i)
        {
            if (lastFullIndex < 0)
            {
                Debug.LogWarning("Player Bullets UI is trying to decrease a non-existant bullet.");
                return;
            }

            UpdateSprite(bulletList[lastFullIndex], emptyBulletSprite);
            --lastFullIndex;
        }
    }

    /// <summary>
    /// UI update on Bullets increase.
    /// </summary>
    /// <param name="amnt">The amount of Bullets that was increased.</param>
    public void IncreaseBullets(int amnt)
    {
        if (maxBullets <= 0)
            SetupUI(amnt);
        else
        {
            for (int i = 0; i < amnt; ++i)
            {
                if (lastFullIndex >= maxBullets - 1)
                {
                    Debug.LogWarning("Player Bullets UI is trying to add additional Bullets.");
                    return;
                }
                else
                {
                    UpdateSprite(bulletList[lastFullIndex + 1], fullBulletSprite);
                    ++lastFullIndex;
                }
            }
        }
    }

    /// <summary>
    /// Sets up the UI to have all necessary bullets.
    /// </summary>
    /// <param name="amnt">The max Bullets; the number of bullets to display on screen.</param>
    private void SetupUI(int amnt)
    {
        if (amnt <= 0)
        {
            Debug.LogWarning("Bullets UI trying to setup with a Bullets of 0!");
            return;
        }

        maxBullets = amnt;
        UpdateSprite(initialBullet, fullBulletSprite);

        //Initialize the list with a capacity of maxBullets;
        bulletList = new List<Image>(maxBullets);
        bulletList.Add(initialBullet);
        lastFullIndex = maxBullets - 1;

        for (int i = 0; i < maxBullets - 1; ++i)
        {
            //Clones the initial bullet.
            bulletList.Add(Instantiate(initialBullet, transform).GetComponent<Image>());
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
