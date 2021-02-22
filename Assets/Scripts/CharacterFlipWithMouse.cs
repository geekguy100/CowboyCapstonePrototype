/*****************************************************************************
// File Name :         CharacterFlipWithMouse.cs
// Author :            Kyle Grenier
// Creation Date :     02/21/2021
//
// Brief Description : Flips a character based on the position of the mouse.
*****************************************************************************/
using UnityEngine;

public class CharacterFlipWithMouse : CharacterFlipper
{
    /// <summary>
    /// Checks to see if the mouse is in front of or behind the character, and flips them accordingly.
    /// </summary>
    public override void CheckDirection(Vector3 dir)
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0f;

        Vector3 direction = mousePos - transform.position;
        base.CheckDirection(direction);
    }
}
