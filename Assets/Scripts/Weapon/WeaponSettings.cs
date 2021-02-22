/*****************************************************************************
// File Name :         WeaponSettings.cs
// Author :            Kyle Grenier
// Creation Date :     02/20/2021
//
// Brief Description : Settings that define how a weapon behaves.
*****************************************************************************/
using UnityEngine;

[CreateAssetMenu(fileName = "Weapon", menuName ="ScriptableObjects/Weapon")]
public class WeaponSettings : ScriptableObject
{
    [Header("Basic Settings")]


    [Tooltip("The weapon's name.")]
    public string weaponName;

    [Tooltip("The bullet the weapon shoots.")] 
    public GameObject bulletPrefab;



    [Header("Shooting")]


    [Tooltip("The weapon's magazine size.")] 
    public int magazineSize;

    [Tooltip("The time in seconds it takes to reload the wepaon.")] 
    public float reloadTime;

    [Tooltip("The weapon's rate of fire. Recommended to keep at 0 for weapons that are burst firing, but can act as a buffer between " +
        "the time waited after bursting and to start shooting again.")] 
    public float timeBetweenShots;



    [Header("Burst Firing")]

    [Tooltip("How many bullets are fired per burst (or shot if burstBulletDelay = 0).")]
    public int bulletsPerBurstFire;

    [Tooltip("The time between spawning consecutive burst bullets.")]
    public float burstBulletDelay;

    [Tooltip("The time to wait between consecutive weapon bursts.")]
    public float burstDelayTime;

    [Tooltip("How many times the weapon will burst fire.")]
    public int roundsOfBurst;

    [Tooltip("The time to wait after burst firing the weapon.")]
    public float timeAfterBurst;


    [Header("Weapon Spread")]


    [Tooltip("The number that determines the spread of the weapon.")] 
    public float bloomMod = 16f;

    [Tooltip("The amount the bullet will sway after calculations.")] 
    public float bloomValue = 8f;

    [Tooltip("The maximum amount of bloom.")] 
    public float maximumBloom = 3f;
}
