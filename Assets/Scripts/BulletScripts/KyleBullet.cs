/*****************************************************************************
// File Name :         KyleBullet.cs
// Author :            Kyle Grenier
// Creation Date :     #CREATIONDATE#
//
// Brief Description : ADD BRIEF DESCRIPTION OF THE FILE HERE
*****************************************************************************/
using UnityEngine;

public class KyleBullet : Bullet
{
    [Tooltip("Time the bullet will be destroyed without hitting anything.")]
    [SerializeField] private float destroyTime = 10f;

    [Tooltip("Returns true if bullet has passed over cover.")]
    public bool passedOverCover = false;

    [Tooltip("Returns true if bullet is passing over cover")]
    public bool passingOverCover = false;

    [Tooltip("counts down the time before a bullet encounters cover")]
    public float encounterCoverTimer = .01f;



    private void Start()
    {
        Destroy(gameObject, destroyTime);
       
    }

    private void Update()
    {
        transform.position += transform.up * speed * Time.deltaTime;

        //if (!passingOverCover)
        //{
        //    encounterCoverTimer -= Time.deltaTime;

        //    if (encounterCoverTimer <= 0)
        //    {
        //        passedOverCover = true;
        //    }
        //}
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        Health health = col.gameObject.GetComponent<Health>();
        ObstacleScript obstacleScript = col.GetComponent<ObstacleScript>();

        //If what we collided with has a Health component, hurt it and destory the bullet.
        if (health != null)
        {
            // Don't collide with an enemy 
            if (isEnemyBullet && col.CompareTag("Enemy"))
            {
                Destroy(gameObject);
                return;
            }

            Destroy(gameObject);
            health.TakeDamage(characterDamage);
        }
        else if (obstacleScript != null /*&& passedOverCover*/)
        {
            Destroy(gameObject);
            obstacleScript.TakeDamage(coverDamage);
        }

        
        //else if (obstacleScript != null && !passedOverCover)
        //{
        //    passingOverCover = true;
        //}
    }
}
