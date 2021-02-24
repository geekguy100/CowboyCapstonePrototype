/*****************************************************************************
// File Name :         PlaytestEnemyPatrol.cs
// Author :            Kyle Grenier
// Creation Date :     02/10/2021
//
// Brief Description : Enemy travels from point to point over time.
*****************************************************************************/
using UnityEngine;
using System.Collections;

public class PlaytestEnemyPatrol : PlaytestEnemyBehaviour
{
    [SerializeField] private Transform[] points;

    [Tooltip("The minimum distance the enemy needs to be to the nearest point to start traveling to the next.")]
    [SerializeField] private float minPointDistance = 2f;

    protected override void PerformAction()
    {
        if (points.Length < 1)
        {
            Debug.Log(gameObject.name + " wants to walk a path but no path points have been assigned! Ignoring...");
            return;
        }

        StartCoroutine("WalkRoute");
    }

    /// <summary>
    /// Moves along a pre-defined set of points over time.
    /// </summary>
    private IEnumerator WalkRoute()
    {
        while (true)
        {
            for (int i = 0; i < points.Length; ++i)
            {
                //Move to closest point.
                while (Vector2.Distance(transform.position, points[i].position) > minPointDistance)
                {
                    Vector2 dir = (points[i].position - transform.position).normalized;
                    characterMovement.Move(dir);
                    yield return null;
                }
            }

            for (int i = points.Length - 1; i > -1; --i)
            {
                //Move to closest point.
                while (Vector2.Distance(transform.position, points[i].position) > minPointDistance)
                {
                    Vector2 dir = (points[i].position - transform.position).normalized;
                    characterMovement.Move(dir);
                    yield return null;
                }
            }
        }
    }


}
