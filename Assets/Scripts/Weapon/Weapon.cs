/*****************************************************************************
// File Name :         Weapon.cs
// Author :            Kyle Grenier
// Creation Date :     #CREATIONDATE#
//
// Brief Description : A basic weapon class that a character can use to shoot bullets.
*****************************************************************************/
using UnityEngine;
using System.Collections;

public class Weapon : MonoBehaviour
{
    [Tooltip("The weapon's settings.")]
    [SerializeField] private WeaponSettings weaponSettings;

    // The name of the weapon.
    private string weaponName;

    // The bullet prefab to instantiate.
    private GameObject bulletPrefab;

    [Tooltip("The origin of the bullet spawn.")]
    [SerializeField] private Transform bulletOrigin;

    // How many bullets this weapon can hold per magazine.
    private int magazineSize;

    // The amount of ammo the player has in the current magazine.
    private int ammoInMagazine;

    // The amount of ammo that is in the player's magazine after reloading.
    private int clipSize;

    private bool reloading = false;

    [Tooltip("The amount of ammo the weapon holder has on their person (on standby).")]
    [SerializeField] private int ammoOnCharacter;

    // The amount of time in seconds it takes to reload the weapon.
    private float reloadTime;

    // The amount of time in seconds that must be waited before being able to shoot again.
    private float timeBetweenShots = 0.3f;
    private float currentShotTime;

    [Tooltip("True if this weapon should have infinite ammo.")]
    [SerializeField] private bool infiniteAmmo = false;

    void Start()
    {
        if (weaponSettings == null)
        {
            Debug.LogWarning("No WeaponSettings found for " + gameObject.name + " of parent " + transform.parent.name + ".");
            Destroy(this);
            return;
        }

        weaponName = weaponSettings.name;
        bulletPrefab = weaponSettings.bulletPrefab;
        magazineSize = weaponSettings.magazineSize;
        reloadTime = weaponSettings.reloadTime;
        timeBetweenShots = weaponSettings.timeBetweenShots;

        ammoInMagazine = magazineSize;
        clipSize = ammoInMagazine;
        currentShotTime = timeBetweenShots;

        // Updating the actual game object's name to that of the given weapon name.
        gameObject.name = weaponName;
    }

    private void Update()
    {
        // Increase the shot timer; it gets set to 0 once a shot has been taken, 
        // and increases until we can take a shot again.
        if (currentShotTime < timeBetweenShots)
            currentShotTime += Time.deltaTime;
    }

    /// <summary>
    /// Fires a bullet from the weapon.
    /// </summary>
    /// <param name="bulletRotation">The rotation to instantiate the bullet at.</param>
    public void Shoot(Quaternion bulletRotation)
    {
        // Taking rate of fire into account.
        if (currentShotTime < timeBetweenShots)
            return;

        print("Shooting weapon");
        if (ammoInMagazine > 0)
        {
            Instantiate(bulletPrefab, bulletOrigin.position, bulletRotation);
            --ammoInMagazine;

            currentShotTime = 0;
        }

    }

    /// <summary>
    /// Reloads the weapon.
    /// </summary>
    public void Reload()
    {
        if (reloading || ammoInMagazine == magazineSize)
            return;

        StartCoroutine(ReloadCoroutine());
    }

    /// <summary>
    /// Waits time reloadTime then updates the ammo in the magazine to 
    /// </summary>
    private IEnumerator ReloadCoroutine()
    {
        reloading = true;
        yield return new WaitForSeconds(reloadTime);

        if (!infiniteAmmo)
        {
            //How many bullets were shot. This should be the amount to refill the weapon with.
            int ammoTaken = clipSize - ammoInMagazine;
            int refillAmount = ammoTaken;

            //If there are not enough bullets on standby to completely fill up the magazine, just use
            //whatever is on standby.
            if (ammoOnCharacter + ammoInMagazine < magazineSize)
                refillAmount = ammoOnCharacter;

            ammoOnCharacter -= ammoTaken;
            if (ammoOnCharacter < 0)
                ammoOnCharacter = 0;


            //Fill up the magazine with as many bullets as we can.
            ammoInMagazine += refillAmount;

            clipSize = ammoInMagazine;
        }
        // If the weapon has infinite ammo, just set its ammo count to the magazine size.
        else
            ammoInMagazine = magazineSize;

        reloading = false;
    }
}
