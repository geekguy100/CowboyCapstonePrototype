/*****************************************************************************
// File Name :         EnemyIdleBehaviour.cs
// Author :            Kyle Grenier
// Creation Date :     #CREATIONDATE#
//
// Brief Description : ADD BRIEF DESCRIPTION OF THE FILE HERE
*****************************************************************************/
using UnityEngine;
using System.Collections;

public class EnemyIdleBehaviour : EnemyBehaviour
{
    [Header("Idle Behaviour")]
    [SerializeField] private float timePerRotation = 2f;

    protected override void PerformAction()
    {
        StartCoroutine("RotateOverTime");
    }

    private IEnumerator RotateOverTime()
    {
        while (!playerInRange)
        {
            yield return new WaitForSeconds(timePerRotation);
            Debug.Log("Enemy rotates!");
            //TODO: Change sprite depending on direction.
        }
    }
}
