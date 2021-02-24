/*****************************************************************************
// File Name :         CoverHelper.cs
// Author :            Kyle Grenier
// Creation Date :     02/24/2021
//
// Brief Description : A static helper class used to deal with finding cover objects.
*****************************************************************************/
using UnityEngine;
using System.Linq;

public static class CoverHelper
{
    /// <summary>
    /// Get the closest cover object from a position.
    /// </summary>
    /// <param name="position">The position to find the closest cover object from.</param>
    public static Cover GetNearestCover(Vector3 position)
    {
        GameObject[] covers = GameObject.FindGameObjectsWithTag("Obstacle");
        GameObject nearestCover = covers
            .OrderBy(t => Vector2.Distance(position, t.transform.position))
            .FirstOrDefault();

        if (nearestCover != null)
            return nearestCover.GetComponent<Cover>();
        else
            return null;
    }

    /// <summary>
    /// AStar has to rescan the scene if a cover gets destroyed or else the
    /// enemies will still think they can't walk on those tiles.
    /// </summary>
    public static void OnCoverDestroyed()
    {
        AstarPath.active.Scan();
    }
}