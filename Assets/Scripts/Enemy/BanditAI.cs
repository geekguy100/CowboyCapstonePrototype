/*****************************************************************************
// File Name :         BanditAI.cs
// Author :            Kyle Grenier
// Creation Date :     02/23/2021
//
// Brief Description : Bandit initially seeks cover and if cover is destroyed will find the next closest.
*****************************************************************************/
using UnityEngine;

public class BanditAI : EnemyAI
{
    [Tooltip("The weapon this bandit uses.")]
    [SerializeField] private Weapon weapon;

    // The cover object used by this 
    private Cover cover = null;

    protected override void Start()
    {
        base.Start();
        // Find a new cover object to hide behind.
        FindNewCover();
    }

    /// <summary>
    /// Regular Bandit will take shots at the player if they are in range.
    /// </summary>
    /// <param name="player">The Transform component of the player.</param>
    protected override void PlayerRangeAction(Transform player)
    {
        weapon.Shoot(player.position, transform);
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

    private void OnDisable()
    {
        if (cover != null)
            cover.OnCoverDestroyed -= FindNewCover;
    }
}