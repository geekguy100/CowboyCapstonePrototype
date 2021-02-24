using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleScript : MonoBehaviour
{
    [Tooltip("Destructable?")]
    public bool destructable = false;

    [Tooltip("The amount of health the object has")]
    public int health = 5;

    //[Tooltip("The particle system to create when damaged or destroyed")]
    //public GameObject particleSystem;

    public void TakeDamage(int damNum)
    {
        health -= damNum;

        if (health <= 0)
        {
            //insert particle system here
            Destroy(gameObject);
        }
    }
}
