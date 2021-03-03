/*****************************************************************************
// File Name :         Cover.cs
// Author :            Kyle Grenier
// Creation Date :     02/24/2021
//
// Brief Description : Script that controls functionality of cover objects.
*****************************************************************************/
using UnityEngine;
using System;

public class Cover : MonoBehaviour
{
    // Event to invoke if this cover object is destroyed.
    public event Action OnCoverDestroyed;

    [Tooltip("The location enemies will navigate towards " +
        "when they want to hide behind this cover.")]
    [SerializeField] private Transform hidingSpot;

    private bool appQuitting = false;

    private void Awake()
    {
        Application.quitting += () => { appQuitting = true; };
    }

    private void OnDestroy()
    {
        // Don't call all this stuff if the player is quitting the application.
        if (!appQuitting)
        {
            // Rescan the scene to update valid enemy paths.
            CoverHelper.OnCoverDestroyed();
            OnCoverDestroyed?.Invoke();
        }
    }

    /// <summary>
    /// Gets the cover's hiding spot.
    /// </summary>
    /// <returns> The location enemies will navigate towards.</returns>
    public Transform GetHidingSpot()
    {
        return hidingSpot;
    }
}