/*****************************************************************************
// File Name :         Weapon.cs
// Author :            Kyle Grenier
// Creation Date :     02/21/2021
//
// Brief Description : A basic weapon class that a character can use to shoot bullets.
*****************************************************************************/
using UnityEngine;
using System.Collections;
using System;

[RequireComponent(typeof(AudioSource))]
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

    // How much damage this weapon does to other characters.
    private int characterDamage;

    // How much damage this weapon does to cover objects.
    private int coverDamage;
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

    private AudioSource audioSource;

    private AudioClip reloadClip;
    private AudioClip shootClip;
    #endregion

    public event Action OnMagazineEmpty;

    public event Action OnReloadStart;
    public event Action OnReloadComplete;

    public event Action OnAmmoPickup;

    public event Action<int> OnWeaponFire;

    // A nearby crate. Used so we don't shoot over cover we're close to.
    [HideInInspector]public GameObject coverToIgnore;

    [Tooltip("If this weapon is an enemy's weapon, bullets will not collide with other enemies.")]
    [SerializeField] private bool isEnemyWeapon = false;

    void Awake()
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
        characterDamage = weaponSettings.characterDamage;
        coverDamage = weaponSettings.coverDamage;
        reloadClip = weaponSettings.reloadingSound;
        shootClip = weaponSettings.shootingSound;

        ammoInMagazine = magazineSize;
        clipSize = ammoInMagazine;
        currentShotTime = timeBetweenShots;

        audioSource = GetComponent<AudioSource>();

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
        if (currentShotTime < timeBetweenShots || isFiring || reloading)
            return;

        //print(currentShotTime + " should be greater than " + timeBetweenShots + " to shoot...");
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
            if (ammoInMagazine > 0)
                audioSource.PlayOneShot(shootClip);

            for (int j = 0; j < bulletsPerBurstFire; ++j)
            {
                if (ammoInMagazine > 0)
                {
                    Quaternion bulletRotation = GetBulletRotation(targetPosition);

                    //get the adjustment value for setting the weapon's spread.
                    float adjVal = UnityEngine.Random.Range(-bloomValue, bloomValue);

                    //set the rotation of the shot, factoring in bloom.
                    bulletRotation *= Quaternion.AngleAxis(adjVal, Vector3.forward);

                    //increase recoil time
                    recoilTime += recoilAmount;

                    Bullet bullet = Instantiate(bulletPrefab, bulletOrigin.position, bulletRotation).GetComponent<Bullet>();
                    if (bullet == null)
                    {
                        Debug.Log(gameObject.name + " of parent " + transform.parent.name + " cannot fire a bullet bc there is no KyleBullet attached to it.");
                        yield break;
                    }

                    bullet.Init(characterDamage, coverDamage, isEnemyWeapon); // Initialize the bullet to deal characterDamage to characters and coverDamage to cover objects.

                    Physics2D.IgnoreCollision(weaponHolder.GetComponent<Collider2D>(), bullet.GetComponent<Collider2D>());

                    // If we have a cover to ignore, make sure to ignore collision with it.
                    if (coverToIgnore != null)
                    {
                        foreach (Collider2D collider in coverToIgnore.GetComponents<Collider2D>())
                            Physics2D.IgnoreCollision(collider, bullet.GetComponent<Collider2D>(), true);
                    }

                    --ammoInMagazine;
                    OnWeaponFire?.Invoke(1);
                }
                else if (!reloading)
                {
                    OnMagazineEmpty?.Invoke();
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
        //BaseShooting.ReloadSound.Play();
        reloading = true;
        OnReloadStart?.Invoke();
        StartCoroutine(ReloadCoroutine());
    }

    /// <summary>
    /// Waits time reloadTime then updates the ammo in the magazine to 
    /// </summary>
    int refillAmount;
    private IEnumerator ReloadCoroutine()
    {
        if (ammoOnCharacter > 0 && reloadClip != null)
            audioSource.PlayOneShot(reloadClip);

        yield return new WaitForSeconds(reloadTime);

        if (!infiniteAmmo)
        {
            //How many bullets were shot. This should be the amount to refill the weapon with.
            int ammoTaken = clipSize - ammoInMagazine;
            refillAmount = ammoTaken;

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
        OnReloadComplete?.Invoke();
    }

    /// <summary>
    /// Gets the weapo's bullet prefab.
    /// </summary>
    /// <returns>he weapon's bullet prefab.</returns>
    public GameObject GetBulletPrefab()
    {
        return bulletPrefab;
    }

    public int GetMagSize()
    {
        return magazineSize;
    }

    public int GetRefillAmount()
    {
        return refillAmount;
    }

    public int GetAmmoOnCharacter()
    {
        return ammoOnCharacter;
    }

    public void AddAmmoToCharacter(int amnt)
    {
        ammoOnCharacter += amnt;
        OnAmmoPickup?.Invoke();
    }

    public void ChangePosition(Vector2 pos)
    {
        transform.position = pos;
    }

    /// <summary>
    /// Forcefully modified the weapon's timeBetweenShots. Used so I don't have 
    /// to make unique weapons for the duelist, who's timeBetweenShots if often
    /// lower than the weapon's original timeBetweenShots.
    /// </summary>
    /// <param name="timeBetweenShots">The weapon's new timeBetweenShots: the time 
    /// that must be waited in between consecutive shots.</param>
    public void UpdateTimeBetweenShots(float timeBetweenShots)
    {
        this.timeBetweenShots = timeBetweenShots;
    }
}
