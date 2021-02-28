/*****************************************************************************
// File Name :         DuelistAI.cs
// Author :            Kyle Grenier
// Creation Date :     02/26/2021
//
// Brief Description : Duelist has two pistols that he fires.
*****************************************************************************/
using UnityEngine;
using System.Collections;

public class DuelistAI : EnemyAI
{
    [Header("The two weapons this duelist will use.")]
    [SerializeField] private Weapon weaponOne;
    [SerializeField] private Weapon weaponTwo;

    [Tooltip("The amount of time to wait before the second weapon is fired.")]
    [SerializeField] private float secondShotDelayTime = 0.5f;

    // True if the duelist is shooting.
    private bool shooting = false;

    // True if the duelist is allowed to shoot.
    // This will be false if he is moving between cover.
    private bool canShoot = false;

    // The cover we want to move to or are currently at.
    private Cover currentCover = null;

    // The other cover that we'll move to when our current cover
    // breaks or we're reloading.
    private Cover otherCover = null;

    [SerializeField] private float timeBetweenShots;

    #region --- Subscribing and Unsubscribing to events. ---
    private void OnEnable()
    {
        weaponTwo.OnReloadStart += SwapCoverPositions;

        // Make sure the enemy knows to reload when he's out of ammo!
        weaponOne.OnMagazineEmpty += weaponOne.Reload;
        weaponTwo.OnMagazineEmpty += weaponTwo.Reload;

        // When the duelist is at his destination, make sure he knows he is able to shoot.
        OnPathfindingComplete += () => 
        {
            canShoot = true;
        };
    }

    private void OnDisable()
    {
        // Make sure the cover events are unsubbed from or else we'll get an error.
        if (currentCover != null)
            currentCover.OnCoverDestroyed -= FindNewCover;
        if (otherCover != null)
            otherCover.OnCoverDestroyed -= FindNewCover;

        weaponTwo.OnReloadStart -= SwapCoverPositions;

        weaponOne.OnMagazineEmpty -= weaponOne.Reload;
        weaponTwo.OnMagazineEmpty -= weaponTwo.Reload;
    }

    #endregion


    protected override void Start()
    {
        base.Start();
        // We want to update the time between shots so the duelist can "rapid fire."
        weaponOne.UpdateTimeBetweenShots(timeBetweenShots);
        weaponTwo.UpdateTimeBetweenShots(timeBetweenShots);
        FindNewCover();
    }

    /// <summary>
    /// Fire both weapons at the player when the player is nearby.
    /// </summary>
    /// <param name="player">The player's Transform component.</param>
    protected override void PlayerRangeAction(Transform player)
    {
        if (!shooting && canShoot)
        {
            shooting = true;
            StartCoroutine(Shoot(player));
        }
    }

    private IEnumerator Shoot(Transform player)
    {
        if (player != null)
            weaponOne.Shoot(player.position, transform);

        yield return new WaitForSeconds(secondShotDelayTime);

        if (player != null)
            weaponTwo.Shoot(player.position, transform);

        yield return new WaitForSeconds(secondShotDelayTime);

        shooting = false;
    }

    /// <summary>
    /// Finds the nearest cover object and sets the Bandit's destination to
    /// that object.
    /// </summary>
    private void FindNewCover()
    {
        print(gameObject.name + " Finding new cover");

        #region --- Old Implementation ---
        //// If we don't have a current cover, but we have a backup to go to,
        //// Set the backup cover as our current one and find a new backup.
        //if (currentCover == null && otherCover != null)
        //{
        //    print("DUELIST: currentCover null and otherCover NOT null");
        //    otherCover.OnCoverDestroyed -= FindNewCover;

        //    currentCover = otherCover;


        //    otherCover = CoverHelper.GetXNearestCover(transform.position, 2);
        //    currentCover.OnCoverDestroyed += FindNewCover;

        //    if (otherCover != null)
        //        otherCover.OnCoverDestroyed += FindNewCover;

        //    StartPathfinding(currentCover.GetHidingSpot());
        //}

        //// If we don't have a current cover and we don't have a backup one,
        //// Find suitable cover objects for both.
        //else if (currentCover == null && otherCover == null)
        //{
        //    print("DUELIST: currentCover null and otherCover null");
        //    currentCover = CoverHelper.GetNearestCover(transform.position);
        //    otherCover = CoverHelper.GetXNearestCover(transform.position, 2);

        //    currentCover.OnCoverDestroyed += FindNewCover;
        //    otherCover.OnCoverDestroyed += FindNewCover;

        //    StartPathfinding(currentCover.GetHidingSpot());
        //}

        //// If we have a current cover but no backup,
        //// Find a new backup cover; don't bother pathfinding since we are safe behind our current cover.
        //else if (currentCover != null && otherCover == null)
        //{
        //    print("DUELIST: currentCover NOT null and otherCover null");
        //    otherCover = CoverHelper.GetXNearestCover(transform.position, 2);

        //    currentCover.OnCoverDestroyed -= FindNewCover;
        //    if (otherCover != null)
        //        otherCover.OnCoverDestroyed += FindNewCover;
        //}
        //else
        //{
        //    Debug.Log("WHY IS THIS CALLED??");
        //}
        #endregion

        if (currentCover != null)
            currentCover.OnCoverDestroyed -= FindNewCover;
        if (otherCover != null)
            otherCover.OnCoverDestroyed -= FindNewCover;

        currentCover = CoverHelper.GetNearestCover(transform.position);
        otherCover = CoverHelper.GetXNearestCover(transform.position, 2);

        if (currentCover != null)
            currentCover.OnCoverDestroyed += FindNewCover;
        if (otherCover != null)
            otherCover.OnCoverDestroyed += FindNewCover;

        // If we never found a suitable cover object to navigate to,
        // Log a warning.
        if (currentCover == null)
        {
            Debug.LogWarning(gameObject.name + ": Cannot find anymore cover objects to go to!");
        }
        else
            StartPathfinding(currentCover.GetHidingSpot());
    }

    /// <summary>
    /// Pathfinds the duelist to the other cover. 
    /// Does this while he's reloading.
    /// </summary>
    private void SwapCoverPositions()
    {
        if (currentCover == null)
        {
            if (otherCover == null)
            {
                Debug.LogWarning("Duelist has no current cover or other cover to navigate to!");
                return;
            }

            StartPathfinding(otherCover.GetHidingSpot());
            return;
        }

        canShoot = false;

        // Swap our backup cover out with our current cover: the cover we plan to go to
        // or are currently at.
        Cover temp = currentCover;
        currentCover = otherCover;
        otherCover = temp;

        StartPathfinding(currentCover.transform);
    }
}
