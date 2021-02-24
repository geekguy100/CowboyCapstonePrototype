/*****************************************************************************
// File Name :         DuelistAI.cs
// Author :            Kyle Grenier
// Creation Date :     #CREATIONDATE#
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

    /// <summary>
    /// Fire both weapons at the player when the player is nearby.
    /// </summary>
    /// <param name="player">The player's Transform component.</param>
    protected override void PlayerRangeAction(Transform player)
    {
        if (!shooting)
        {
            shooting = true;
            StartCoroutine(Shoot(player));
        }
    }

    private IEnumerator Shoot(Transform player)
    {
        weaponOne.Shoot(player.position, transform);
        yield return new WaitForSeconds(secondShotDelayTime);
        weaponTwo.Shoot(player.position, transform);
        shooting = false;
    }
}
