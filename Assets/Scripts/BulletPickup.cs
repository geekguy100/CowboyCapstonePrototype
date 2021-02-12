using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPickup : MonoBehaviour
{
    public int bulletCount = 12;

    void OnTriggerEnter2D(Collider2D col)
    {
        Debug.Log("asdf");
        BaseShooting bs = col.GetComponentInChildren<BaseShooting>();
        if (bs != null)
        {
            bs.extraBulletCount += bulletCount;
            Destroy(gameObject);
        }
    }
}
