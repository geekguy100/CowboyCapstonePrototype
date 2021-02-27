using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCollider : MonoBehaviour
{
    public bool collisionDetectionMode = false;


    void OnTriggerEnter2D(Collider2D col)
    {
        //detect enemies
        if (collisionDetectionMode)
        {
            switch (col.gameObject.tag)
            {
                case "Enemy":
                
                    break;
                    /*
                case "Obstacle":
                    //destroy self
                    Destroy(transform.parent.gameObject);
                    break;
                    */
            }
        }
        else//detect player
        {

            switch (col.gameObject.tag)
            {
                case "Player":

                    break;
                case "Obstacle":
                    //destroy self
                    Destroy(transform.parent.gameObject);
                    break;
            }
        }
    }
}
