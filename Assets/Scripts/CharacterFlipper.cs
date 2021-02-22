/*****************************************************************************
// File Name :         SpriteFlipper.cs
// Author :            Kyle Grenier
// Creation Date :     02/21/2021
//
// Brief Description : Allows a sprite to flip.
*****************************************************************************/
using UnityEngine;

public class CharacterFlipper : MonoBehaviour
{
    // True if the character is facing right.
    protected bool facingRight = false;

    /// <summary>
    /// Checks if the character is facing in the right direction and flips it if it is not.
    /// </summary>
    /// <param name="dir">The direction the character should be facing.</param>
    public virtual void CheckDirection(Vector3 dir)
    {
        if (dir.x > 0 && !facingRight)
        {
            Flip();
        }
        else if (dir.x < 0 && facingRight)
        {
            Flip();
        }
    }

    /// <summary>
    /// Flips the direction of the character.
    /// </summary>
    protected void Flip()
    {
        facingRight = !facingRight;

        Vector3 scale = transform.localScale;
        scale.x = -scale.x;

        transform.localScale = scale;
    }
}
