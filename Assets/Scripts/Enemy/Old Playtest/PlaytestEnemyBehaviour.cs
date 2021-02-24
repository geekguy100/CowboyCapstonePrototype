/*****************************************************************************
// File Name :         PlaytestEnemyBehaviour.cs
// Author :            Kyle Grenier
// Creation Date :     #CREATIONDATE#
//
// Brief Description : ADD BRIEF DESCRIPTION OF THE FILE HERE
*****************************************************************************/
using UnityEngine;

[RequireComponent(typeof(CharacterMovement))]
public abstract class PlaytestEnemyBehaviour : MonoBehaviour
{
    protected CharacterMovement characterMovement;

    protected GameObject player;

    /// <summary>
    /// The unique action this behaviour performs.
    /// </summary>
    protected abstract void PerformAction();

    protected virtual void Awake()
    {
        characterMovement = GetComponent<CharacterMovement>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    protected virtual void Start()
    {
        //Perform action on start.
        PerformAction();
    }
}