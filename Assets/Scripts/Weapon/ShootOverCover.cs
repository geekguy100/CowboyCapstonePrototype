/*****************************************************************************
// File Name :         ShootOverCover.cs
// Author :            Kyle Grenier
// Creation Date :     02/27/2021
//
// Brief Description : Controls behaviour for shooting over cover.
*****************************************************************************/
using UnityEngine;

public class ShootOverCover : MonoBehaviour
{ 
    private void OnTriggerEnter2D(Collider2D col)
    {
        Weapon w = col.GetComponentInChildren<Weapon>();
        if (w != null)
        {
            w.coverToIgnore = transform.parent.gameObject;
            print("IGNORING COLLISION.");
        }
 
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        Weapon w = col.GetComponentInChildren<Weapon>();
        if (w != null)
        {
            w.coverToIgnore = null;
            print("COLLISION BACK ON.");
        }
    }
}
