using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPickup : MonoBehaviour
{
    public int bulletCount = 12;
    private BulletCountUI bcui;


    void Awake()
    {
        bcui = FindObjectOfType<BulletCountUI>();
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        
        Weapon weapon = col.GetComponentInChildren<Weapon>();
        if (weapon != null && col.CompareTag("Player"))
        {
            weapon.AddAmmoToCharacter(bulletCount);
            Destroy(gameObject);
        }
    }
}
