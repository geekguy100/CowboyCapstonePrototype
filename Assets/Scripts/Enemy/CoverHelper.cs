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
    /// Get the nearest cover object from a position.
    /// </summary>
    /// <param name="position">The position to find the nearest cover object from.</param>
    /// <returns>The nearest cover object.</returns>
    public static Cover GetNearestCover(Vector3 position)
    {
        GameObject[] covers = GameObject.FindGameObjectsWithTag("Obstacle");
        GameObject nearestCover = covers
            .OrderBy(t => Vector2.Distance(position, t.transform.position))
            .Where(t => t.activeInHierarchy)
            .FirstOrDefault();

        if (nearestCover != null)
            return nearestCover.GetComponent<Cover>();
        else
            return null;
    }

    /// <summary>
    /// Get the x-nearest cover object from a position.
    /// </summary>
    /// <param name="position">The position to find the nearest cover object from.</param>
    /// <param name="x">The number signifying the x-nearest from to find from the given position.</param>
    /// <returns>The x-nearest cover object.</returns>
    public static Cover GetXNearestCover(Vector3 position, int x)
    {
        int index = x - 1;

        // I don't want to have to type in "the 0th nearest cover" for example,
        // so I'll just subtract 1 from the index.
        if (index < 0)
        {
            Debug.LogWarning("CoverHelper: index too low. Returning null.");
            return null;
        }

        GameObject[] covers = GameObject.FindGameObjectsWithTag("Obstacle");
        GameObject xNearestCover = covers
            .OrderBy(t => Vector2.Distance(position, t.transform.position))
            .Where(t => t.activeInHierarchy)
            .ElementAtOrDefault(index);

        if (xNearestCover != null)
            return xNearestCover.GetComponent<Cover>();
        else
            return null;
    }

    /// <summary>
    /// AStar has to rescan the scene if a cover gets destroyed or else the
    /// enemies will still think they can't walk on those tiles.
    /// </summary>
    public static void OnCoverDestroyed()
    {
        if (AstarPath.active != null)
            AstarPath.active.Scan();
    }
}