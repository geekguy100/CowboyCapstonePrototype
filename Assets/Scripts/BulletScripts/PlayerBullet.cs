using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    [Tooltip("The travel speed of the bullet.")]
    [SerializeField] private float speed = 5f;

    [Tooltip("The damage the bullet inflicts.")]
    [SerializeField] private int damage = 1;

    [Tooltip("Time the bullet will be destroyed without hitting anything.")]
    [SerializeField] private float destroyTime = 10f;


    [Tooltip("Counts down and sets the bullet to destroy if has not encoutered cover")]
    [SerializeField] private float encounteredCoverTimer = .5f;

    bool passingThroughCover = false;
    bool passedOverCover = false;

    private void Start()
    {
        Destroy(gameObject, destroyTime);
    }

    private void Update()
    {
        var time = Time.deltaTime;

        transform.position += transform.up * speed * time;

        if (!passingThroughCover)
        {
            encounteredCoverTimer -= time;
            if (encounteredCoverTimer <= 0)
            {
                passedOverCover = true;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //bullet has exited cover therefore the next collision should destroy bullet
        passedOverCover = true;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        Health health = col.gameObject.GetComponent<Health>();

        //If what we collided with has a Health component, hurt it and destory the bullet.
        if (health != null)
        {
            health.TakeDamage(damage);
            Destroy(gameObject);
        }
        else if (col.CompareTag("Obstacle") && passedOverCover)
        {
            //Uncomment below to allow bullets to destroy obstacles.
            //Destroy(col.gameObject);
            Destroy(gameObject);
        }else if (col.CompareTag("Obstacle") && !passedOverCover)
        {
            passingThroughCover = true;
        }
    }
}
