using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BulletCountUI : MonoBehaviour
{
    TextMeshProUGUI countText;
    private int lastCount;

    Weapon playerWeapon;

    void Awake()
    {
        countText = GetComponent<TextMeshProUGUI>();
        playerWeapon = GameObject.Find("Player").GetComponentInChildren<Weapon>();

        if (playerWeapon == null)
            return;

        countText.text = playerWeapon.GetAmmoOnCharacter().ToString();
        playerWeapon.OnReloadComplete += UpdateCount;
        playerWeapon.OnAmmoPickup += UpdateCount;
    }

    private void OnDisable()
    {
        playerWeapon.OnReloadComplete -= UpdateCount;
        playerWeapon.OnAmmoPickup -= UpdateCount;
    }

    void UpdateCount()
    {
        countText.text = playerWeapon.GetAmmoOnCharacter().ToString();
    }
}
