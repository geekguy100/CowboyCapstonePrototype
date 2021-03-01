using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeBullet : Bullet
{
    [Tooltip("End location of grenade")]
    [SerializeField] private Vector2 endLoc;

    [Tooltip("The amount of time before explosion")]
    [SerializeField] private float explosionTimer = 3;

    [Tooltip("The explosion object")]
    [SerializeField] public GameObject explosion;


    // Start is called before the first frame update
    void Start()
    {

        //obtain location of player
        endLoc = GameObject.Find("Player").transform.position;

        if(endLoc == null)
        {
            Destroy(gameObject);
        }
        //calculate position just before player
        //endLoc = endLoc - ((Vector2)transform.position * .96f);

        Invoke("Explode", 3);
    }

    // Update is called once per frame
    void Update()
    {
        //move towards end position
        transform.position = Vector3.MoveTowards(transform.position, endLoc, speed * Time.deltaTime);
    }

    void Explode()
    {
        Debug.Log("explosion occured");
        explosion.SetActive(true);
    }

}
