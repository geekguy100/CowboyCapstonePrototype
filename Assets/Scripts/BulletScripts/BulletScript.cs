using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    [Tooltip("The originator of the bullet")]
    public GameObject origin; 

    [Tooltip("the speed of the bullet")]
    public float speed = 8;

    [Tooltip("the amount of time the bullet exists")]
    public float bulletDeathTimer = 10;

    [Tooltip("true = bullet was launched by player | false = bullet was launched by enemy")]
    public bool collisionDetectionMode = true;

    private void Awake()
    {
        //set the position of the collider object
        setCollider();
    }

    void Start()
    {
        //determine what to hit
        determineWhatToHit();
    }

    void Update()
    {
        //move bullet forward
        transform.Translate(speed * Time.deltaTime, 0, 0, Space.Self);

        //deduct time from life
        deathTimer();
    }

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
                    Destroy(gameObject);
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
                    Destroy(gameObject);
                    break;
            }
        }
    }

    //timer that will remove bullet after a period of time
    void deathTimer()
    {
        //deduct time
        bulletDeathTimer -= Time.deltaTime;

        //check if timer out
        if (bulletDeathTimer <= 0)
        {
            //destroy self
            Destroy(gameObject);
        }
    }

    //set the position of the ground collider
    void setCollider()
    {
        //this all rotates the collider child object of the bullet

        //obtain rotation of bullet
        Quaternion tempQuat = transform.rotation;

        //zero out the rotation
        transform.rotation = new Quaternion();

        //get the child game object
        GameObject tempObj = this.gameObject.transform.GetChild(0).transform.gameObject;

        //get the transform component of the child object
        Transform tran = tempObj.transform;

        //set the position of the child collider
        tran.position -= new Vector3(0, 1.25f);

        //unchild the collider
        transform.DetachChildren();

        //reset the rotation of the bullet
        transform.rotation = tempQuat;

        //reparent the transform
        tempObj.transform.SetParent(gameObject.transform, true);
    }

    //determine the launcher of the bullet
    void determineWhatToHit()
    {
        //player
        if(origin.CompareTag("Player"))
        {
            collisionDetectionMode = true;
            gameObject.GetComponentInChildren<BulletCollider>().collisionDetectionMode = true;
        }
        else//assume enemy
        {
            collisionDetectionMode = false;
        }
    }
}
