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
    // The weapon's name.
    public string weaponName;

    // The bullet the weapon shoots.
    public GameObject bulletPrefab;

    // The weapon's magazine size.
    public int magazineSize;

    // The time in seconds it takes to reload the wepaon.
    public float reloadTime;

    // The weapon's rate of fire.
    public float timeBetweenShots;
}
