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
        
        BaseShooting bs = col.GetComponentInChildren<BaseShooting>();
        if (bs != null)
        {
            bs.extraBulletCount += bulletCount;
            bcui.IncreaseCount(bulletCount);
            Destroy(gameObject);
        }
    }
}
