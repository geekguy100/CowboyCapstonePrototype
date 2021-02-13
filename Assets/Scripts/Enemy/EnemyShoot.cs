/*****************************************************************************
// File Name :         EnemyShoot.cs
// Author :            Kyle Grenier
// Creation Date :     #CREATIONDATE#
//
// Brief Description : Basic shooting mechancis for the enemy in playtest.
*****************************************************************************/
using UnityEngine;
using System.Collections;

public class EnemyShoot : MonoBehaviour
{
    [Tooltip("The bullet prefab to instantiate.")]
    [SerializeField] private GameObject bulletPrefab;

    [Tooltip("The minimum wait time for enemy to shoot.")]
    [SerializeField] private float minWaitTime = 2f;

    [Tooltip("The maximum wait time for enemy to shoot.")]
    [SerializeField] private float maxWaitTime = 5f;

    [Tooltip("The miniumum distance the enemy needs to be from the player before shooting.")]
    [SerializeField] private float minDistance = 10f;

    private Transform player;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Start()
    {
        StartCoroutine("Shoot");
    }

    private IEnumerator Shoot()
    {
        while(player != null)
        {
            if (Vector2.Distance(transform.position, player.position) > minDistance)
            {
                yield return null;
            }

            //Calculate an angle towards the player.
            Vector2 dir = (player.position - transform.position).normalized;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

            Quaternion rotation = Quaternion.Euler(0, 0, angle - 90);

            GameObject bullet = Instantiate(bulletPrefab, transform.position, rotation);
            Physics2D.IgnoreCollision(bullet.GetComponent<Collider2D>(), GetComponentInParent<Collider2D>()); //Make sure to ignore the collision with the bullet so the enemy won't instantly die.

            yield return new WaitForSeconds(Random.Range(minWaitTime, maxWaitTime));
        }
    }
}
