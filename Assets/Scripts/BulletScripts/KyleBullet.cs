/*****************************************************************************
// File Name :         KyleBullet.cs
// Author :            Kyle Grenier
// Creation Date :     #CREATIONDATE#
//
// Brief Description : ADD BRIEF DESCRIPTION OF THE FILE HERE
*****************************************************************************/
using UnityEngine;

public class KyleBullet : MonoBehaviour
{
    [Tooltip("The travel speed of the bullet.")]
    [SerializeField] private float speed = 5f;

    [Tooltip("The damage the bullet inflicts.")]
    [SerializeField] private int damage = 1;

    [Tooltip("Time the bullet will be destroyed without hitting anything.")]
    [SerializeField] private float destroyTime = 10f;

    private void Start()
    {
        Destroy(gameObject, destroyTime);
    }

    private void Update()
    {
        transform.position += transform.up * speed * Time.deltaTime;
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
        else if (col.CompareTag("Obstacle"))
        {
            //Uncomment below to allow bullets to destroy obstacles.
            //Destroy(col.gameObject);
            Destroy(gameObject);
        }
    }
}
