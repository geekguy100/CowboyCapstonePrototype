using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    //Bullet Speed
    public GameObject origin; 
    public float speed = 8;
    public float bulletDeathTimer = 10;
    
    void Update()
    {
        transform.Translate(speed * Time.deltaTime, 0, 0, Space.Self);
        deathTimer();
    }

    void deathTimer()
    {
        bulletDeathTimer -= Time.deltaTime;
        if (bulletDeathTimer <= 0)
        {
            Destroy(gameObject);
        }
    }
}
