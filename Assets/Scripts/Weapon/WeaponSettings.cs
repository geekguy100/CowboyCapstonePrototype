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
    [Tooltip("The weapon's name.")]
    public string weaponName;

    [Tooltip("The bullet the weapon shoots.")] 
    public GameObject bulletPrefab;

    [Tooltip("The weapon's magazine size.")] 
    public int magazineSize;

    [Tooltip("The time in seconds it takes to reload the wepaon.")] 
    public float reloadTime;

    [Tooltip("The weapon's rate of fire.")] 
    public float timeBetweenShots;

    [Tooltip("The number that determines the spread of the weapon.")] 
    public float bloomMod = 16f;

    [Tooltip("The amount the bullet will sway after calculations.")] 
    public float bloomValue = 8f;

    [Tooltip("The maximum amount of bloom.")] 
    public float maximumBloom = 3f;
}
