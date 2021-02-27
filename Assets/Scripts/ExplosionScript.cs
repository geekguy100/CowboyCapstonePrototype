using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionScript : MonoBehaviour
{
    [Tooltip("The damage the bullet inflicts to player.")]
    [SerializeField] private int characterDamage = 1;

    [Tooltip("The damage the bullet inflicts to cover.")]
    [SerializeField] private int coverDamage = 5;

    private void OnTriggerStay2D(Collider2D col)
    {
        Health health = col.gameObject.GetComponent<Health>();
        ObstacleScript obstacleScript = col.gameObject.GetComponent<ObstacleScript>();

        if (health != null)
        {
            health.TakeDamage(characterDamage);
        }

        if (obstacleScript != null)
        {
            obstacleScript.TakeDamage(coverDamage);
        }
    }

   void Awake()
    {
        Destroy(transform.parent.gameObject, .04f);
        Destroy(gameObject,.04f); 
    }
}
