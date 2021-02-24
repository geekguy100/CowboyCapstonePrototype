/*****************************************************************************
// File Name :         Weapon.cs
// Author :            Kyle Grenier
// Creation Date :     02/21/2021
//
// Brief Description : A basic weapon class that a character can use to shoot bullets.
*****************************************************************************/
using UnityEngine;
using System.Collections;

public class Weapon : MonoBehaviour
{
    #region ------- Weapon Settings - Data obtained from WeaponSettings scriptable object.
    [Tooltip("The weapon's settings.")]
    [SerializeField] private WeaponSettings weaponSettings;

    // The name of the weapon.
    private string weaponName;

    // How many bullets this weapon can hold per magazine.
    private int magazineSize;

    // The amount of time in seconds it takes to reload the weapon.
    private float reloadTime;

    // The amount of time in seconds that must be waited before being able to shoot again.
    private float timeBetweenShots;

    // The bullet prefab to instantiate.
    private GameObject bulletPrefab;

    // Amount of bloom that will sway a bullet after calculations.
    private float bloomValue;

    // Value that determines max and min bloom.
    private float bloomMod;

    // The maxmimum amount of bloom.
    private float maximumBloom;

    // How many bullets are fired per shot.
    private int bulletsPerBurstFire;

    // How many bullets per burst fire.
    private int roundsOfBurst;

    // The time between consecutive burst bullet spawns.
    private float burstBulletDelay;

    // The time to wait between consecutive weapon bursts.
    private float burstDelayTime;

    // The time to wait after burst firing a weapon.
    private float timeAfterBurst;
    #endregion

    #region ------- Serialized Fields - Data set in the inspector.
    [Tooltip("The origin of the bullet spawn.")]
    [SerializeField] private Transform bulletOrigin;

    [Tooltip("The amount of ammo the weapon holder has on their person (on standby).")]
    [SerializeField] private int ammoOnCharacter;

    [Tooltip("True if this weapon should have infinite ammo.")]
    [SerializeField] private bool infiniteAmmo = false;
    #endregion

    #region ------- Internal Data - Private fields used by the weapon to keep track of shot time, ammo, etc.
    // The current amount of time waited after taking a shot.
    private float currentShotTime;

    // The amount of ammo the player has in the current magazine.
    private int ammoInMagazine;

    // The amount of ammo that is in the player's magazine after reloading.
    private int clipSize;

    // True if the weapon is being reloaded.
    private bool reloading = false;

    // Amount of time shot is held, will corelate with bloom.
    private float recoilTime = 0f;

    // Amount of time shot needs to be held to zero in on enemy.
    private float recoilAmount = 1f;

    // True if the weapon is being fired.
    private bool isFiring = false;
    #endregion


    void Start()
    {
        if (weaponSettings == null)
        {
            Debug.LogWarning("No WeaponSettings found for " + gameObject.name + " of parent " + transform.parent.name + ".");
            Destroy(this);
            return;
        }

        // Extract data from scriptable object WeaponSettings.
        weaponName = weaponSettings.name;
        bulletPrefab = weaponSettings.bulletPrefab;
        magazineSize = weaponSettings.magazineSize;
        reloadTime = weaponSettings.reloadTime;
        timeBetweenShots = weaponSettings.timeBetweenShots;
        bloomMod = weaponSettings.bloomMod;
        bloomValue = weaponSettings.bloomValue;
        maximumBloom = weaponSettings.maximumBloom;
        roundsOfBurst = weaponSettings.roundsOfBurst;
        bulletsPerBurstFire = weaponSettings.bulletsPerBurstFire;
        burstBulletDelay = weaponSettings.burstBulletDelay;
        burstDelayTime = weaponSettings.burstDelayTime;
        timeAfterBurst = weaponSettings.timeAfterBurst;

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

        // if recoil is occuring
        if (recoilTime > 0)
        {
            //decrease recoil
            recoilTime -= Time.deltaTime;
        }
        else
        {
            // keep recoil at 0
            recoilTime = 0;
        }

        AdjustBloom();
    }

    //calculate the bloom
    private void AdjustBloom()
    {
        //calculate the bloom values
        bloomValue = recoilTime * bloomMod;

        //if the value is too large reduce it
        if (recoilTime > maximumBloom)
        {
            recoilTime = maximumBloom;
        }
    }

    /// <summary>
    /// Fires a bullet from the weapon.
    /// </summary>
    /// <param name="targetPosition">The position in world space of the target to shoot at.</param>
    /// <param name="weaponHolder">The Transform using the weapon. Used to prevent accidental collision.</param>
    public void Shoot(Vector3 targetPosition, Transform weaponHolder)
    {
        // Taking rate of fire into account.
        if (currentShotTime < timeBetweenShots || isFiring)
            return;

        print(currentShotTime + " should be greater than " + timeBetweenShots + " to shoot...");
        isFiring = true;
        StartCoroutine(ShootCoroutine(targetPosition, weaponHolder));
    }

    /// <summary>
    /// Gets the rotation of the bullet given the target's position.
    /// </summary>
    /// <param name="targetPosition">The position in world space of the target to shoot at.</param>
    /// <returns></returns>
    public Quaternion GetBulletRotation(Vector3 targetPosition)
    {
        Vector3 dir = (targetPosition - bulletOrigin.position).normalized;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        Quaternion bulletRotation = Quaternion.AngleAxis(angle - 90f, Vector3.forward);

        return bulletRotation;
    }
    
    /// <summary>
    /// Get the current bloom value of the weapon.
    /// </summary>
    /// <returns>The weapon's current bloom value.</returns>
    public float GetBloomValue()
    {
        return bloomValue;
    }

    public Transform GetBulletOrigin()
    {
        return bulletOrigin;
    }

    private IEnumerator ShootCoroutine(Vector3 targetPosition, Transform weaponHolder)
    {
        int currentBursts = roundsOfBurst;

        do
        {
            for (int j = 0; j < bulletsPerBurstFire; ++j)
            {
                if (ammoInMagazine > 0)
                {
                    Quaternion bulletRotation = GetBulletRotation(targetPosition);

                    //get the adjustment value for setting the weapon's spread.
                    float adjVal = Random.Range(-bloomValue, bloomValue);

                    //set the rotation of the shot, factoring in bloom.
                    bulletRotation *= Quaternion.AngleAxis(adjVal, Vector3.forward);

                    //increase recoil time
                    recoilTime += recoilAmount;

                    GameObject bullet = Instantiate(bulletPrefab, bulletOrigin.position, bulletRotation);
                    Physics2D.IgnoreCollision(weaponHolder.GetComponent<Collider2D>(), bullet.GetComponent<Collider2D>());

                    --ammoInMagazine;
                }

                yield return new WaitForSeconds(burstBulletDelay);
            }

            --currentBursts;
            yield return new WaitForSeconds(burstDelayTime);

        } while (currentBursts > 0);

        yield return new WaitForSeconds(timeAfterBurst);
        isFiring = false;
        currentShotTime = 0;
    }

    /// <summary>
    /// Reloads the weapon.
    /// </summary>
    public void Reload()
    {
        if (reloading || ammoInMagazine == magazineSize)
            return;

        reloading = true;
        StartCoroutine(ReloadCoroutine());
    }

    /// <summary>
    /// Waits time reloadTime then updates the ammo in the magazine to 
    /// </summary>
    private IEnumerator ReloadCoroutine()
    {
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
