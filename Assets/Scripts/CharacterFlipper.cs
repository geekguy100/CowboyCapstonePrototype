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
    [Tooltip("Sprites are organized in the order of RIGHT, DIAGONAL TOP RIGHT, and DIAGONAL BOTTOM RIGHT.")]
    [SerializeField] private Sprite[] sprites;

    public enum DIRECTION { RIGHT, LEFT, UPRIGHT, UPLEFT, DOWNRIGHT, DOWNLEFT, UPDOWN};
    private DIRECTION directionFacing;

    private SpriteRenderer spriteRenderer;

    [SerializeField] private Transform[] bulletOrigins;

    [Tooltip("Organized in the order of RIGHT, DIAGONAL TOP RIGHT, DIAGONAL TOP LEFT, LEFT, DIAGONAL BOTTOM LEFT, DIAGONAL BOTTOM RIGHT.")]
    [SerializeField] private Transform[] bulletOriginsPositions;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    /// <summary>
    /// Checks if the character is facing in the right direction and flips it if it is not.
    /// </summary>
    /// <param name="dir">The direction the character should be facing.</param>
    public virtual void CheckDirection(Vector3 dir)
    {
        print(dir);
        if ((dir.x > 0 && dir.y > 0) && directionFacing != DIRECTION.UPRIGHT)
        { 
            directionFacing = DIRECTION.UPRIGHT;
            ChangeSprite(sprites[1], true);
        }
        else if ((dir.x > 0 && dir.y < 0) && directionFacing != DIRECTION.DOWNRIGHT)
        {           
            directionFacing = DIRECTION.DOWNRIGHT;
            ChangeSprite(sprites[2], true);
        }
        else if ((dir.x < 0 && dir.y > 0) && directionFacing != DIRECTION.UPLEFT)
        {          
            directionFacing = DIRECTION.UPLEFT;
            ChangeSprite(sprites[1], false);
        }
        else if ((dir.x < 0 && dir.y < 0) && directionFacing != DIRECTION.DOWNLEFT)
        {          
            directionFacing = DIRECTION.DOWNLEFT;
            ChangeSprite(sprites[2], false);
        }
        else if ((dir.x > 0 && dir.y == 0) && directionFacing != DIRECTION.RIGHT)
        {           
            directionFacing = DIRECTION.RIGHT;
            ChangeSprite(sprites[0], true);
        }
        else if ((dir.x < 0 && dir.y == 0) && directionFacing != DIRECTION.LEFT)
        {           
            directionFacing = DIRECTION.LEFT;
            ChangeSprite(sprites[0], false);
        }
    }

    /// <summary>
    /// Changes the sprite to the given sprite.
    /// </summary>
    /// <param name="sprite">The sprite to change to.</param>
    /// <param name="facingRight">True if the character should be facing right.</param>
    private void ChangeSprite(Sprite sprite, bool facingRight)
    {
        spriteRenderer.sprite = sprite;
        Vector3 scale = transform.localScale;

        if (facingRight)
            scale.x = 1;
        else
            scale.x = -1;

        transform.localScale = scale;
    }
}
