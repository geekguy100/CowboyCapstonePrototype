/*****************************************************************************
// File Name :         EnemyFlip.cs
// Author :            Kyle Grenier
// Creation Date :     #CREATIONDATE#
//
// Brief Description : ADD BRIEF DESCRIPTION OF THE FILE HERE
*****************************************************************************/
using UnityEngine;

public class EnemyFlip : CharacterFlipper
{
    /// <summary>
    /// Checks to see if the mouse is in front of or behind the character, and flips them accordingly.
    /// </summary>
    public override void CheckDirection(Vector3 dir)
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            Vector2 direction = player.transform.position - transform.position;

            base.CheckDirection(direction.normalized);
        }
    }
}
