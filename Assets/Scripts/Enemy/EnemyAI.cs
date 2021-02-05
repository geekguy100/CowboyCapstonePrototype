/*****************************************************************************
// File Name :         EnemyAI.cs
// Author :            Kyle Grenier
// Creation Date :     #CREATIONDATE#
//
// Brief Description : EnemyAI that detects when the player is in range, and manages switching between AI behaviours.
*****************************************************************************/

using UnityEngine;
using Pathfinding;

[RequireComponent(typeof(Seeker))]
[RequireComponent(typeof(CharacterMovement))]
public class EnemyAI : MonoBehaviour
{
    private CharacterMovement characterMovement;
    private EnemyBehaviour enemyBehaviour;

    private Transform player;



    private void Awake()
    {
        characterMovement = GetComponent<CharacterMovement>();
        enemyBehaviour = GetComponent<EnemyBehaviour>();

        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        if (Vector2.Distance(transform.position, player.position) < 20f)
        {
            //Player in range of enemy. Switch to combat behaviour.
        }
    }
}
