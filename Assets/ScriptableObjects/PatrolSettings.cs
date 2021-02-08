/*****************************************************************************
// File Name :         PatrolSettings.cs
// Author :            Kyle Grenier
// Creation Date :     #CREATIONDATE#
//
// Brief Description : ScriptableObject 
*****************************************************************************/
using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy Patrol Settings", menuName = "Enemy Patrol Settings")]
public class PatrolSettings : ScriptableObject
{
    //The locations the enemy will patrol across, in order of lowest index to highest index.
    public Vector2[] locations;
}
