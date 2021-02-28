/*****************************************************************************
// File Name :         ShotgunnerAI.cs
// Author :            Kyle Grenier
// Creation Date :     02/27/2021
//
// Brief Description : AI functionality for the Shotgunner enemy - The enemy who will charge the player with a shotgun.
*****************************************************************************/
using UnityEngine;

public class ShotgunnerAI : EnemyAI
{
    [Tooltip("The weapon used by this enemy.")]
    [SerializeField] private Weapon weapon;

    private Cover cover;

    private bool canChasePlayer = true;

    #region --- Subscribing and Unsubscribing to Weapon events ---
    private void OnEnable()
    {
        weapon.OnMagazineEmpty += weapon.Reload;

        weapon.OnReloadStart += FindNewCover;

        // Only chase the player if we are not reloading.
        weapon.OnReloadStart += () => { canChasePlayer = false; };
        weapon.OnReloadComplete += () => { canChasePlayer = true; };
    }

    private void OnDisable()
    {
        weapon.OnMagazineEmpty -= weapon.Reload;

        if (cover != null)
            cover.OnCoverDestroyed -= FindNewCover;
    }
    #endregion


    protected override void Start()
    {
        base.Start();
        FindNewCover();
    }

    /// <summary>
    /// Shotgunner starts pathfinding towards the payer if they are in range.
    /// </summary>
    /// <param name="player">The Transform component of the player game object.</param>
    protected override void PlayerRangeAction(Transform player)
    {
        // If the enemy is not already moving (pathing towards the player) and we can chase, 
        // start pathing towards the player.
        if (!IsMoving && canChasePlayer)
        {
            print("Shotgunner: Starting CHASING.");
            StartPathfinding(player);
        }

        weapon.Shoot(player.position, transform);
    }

    /// <summary>
    /// Once the player leaves the shotgunner's range, he'll seek out cover.
    /// </summary>
    /// <param name="player">The transform component of the player game object.</param>
    protected override void PlayerLeftRangeAction(Transform player)
    {
        FindNewCover();
    }

    /// <summary>
    /// Finds the nearest cover object and sets the Bandit's destination to
    /// that object.
    /// </summary>
    private void FindNewCover()
    {
        print(gameObject.name + ": Finding new cover");
        // Just in case we want to navigate to a new cover even though the current one
        // never got destroyed.
        if (cover != null)
            cover.OnCoverDestroyed -= FindNewCover;

        cover = CoverHelper.GetNearestCover(transform.position);

        if (cover != null)
        {
            cover.OnCoverDestroyed += FindNewCover;

            StartPathfinding(cover.GetHidingSpot());
        }
        else
            Debug.LogWarning(gameObject.name + ": Cannot find anymore cover objects to go to!");
    }
}
