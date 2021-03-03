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
    [Tooltip("Sprites are organized in the order of LEFT, DIAGONAL TOP RIGHT, and DIAGONAL BOTTOM RIGHT.")]
    [SerializeField] private Sprite[] sprites;

    public enum DIRECTION { RIGHT, LEFT, UPRIGHT, UPLEFT, DOWNRIGHT, DOWNLEFT, UPDOWN};
    private DIRECTION directionFacing;

    private SpriteRenderer spriteRenderer;

    [Tooltip("TOP RIGHT, BOTTOM RIGHT)")]
    [SerializeField] private Transform[] bulletOriginsPositions;

    Weapon weapon;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        weapon = GetComponentInChildren<Weapon>();
    }

    /// <summary>
    /// Checks if the character is facing in the right direction and flips it if it is not.
    /// </summary>
    /// <param name="dir">The direction the character should be facing.</param>
    public virtual void CheckDirection(Vector3 dir)
    {
        if ((dir.x > 0 && dir.y > 0) && directionFacing != DIRECTION.UPRIGHT)
        {
            weapon.ChangePosition(bulletOriginsPositions[0].position);
            directionFacing = DIRECTION.UPRIGHT;
            ChangeSprite(sprites[1], true);
        }
        else if ((dir.x > 0 && dir.y < 0) && directionFacing != DIRECTION.DOWNRIGHT)
        {
            weapon.ChangePosition(bulletOriginsPositions[1].position);
            directionFacing = DIRECTION.DOWNRIGHT;
            ChangeSprite(sprites[2], true);
        }
        else if ((dir.x < 0 && dir.y > 0) && directionFacing != DIRECTION.UPLEFT)
        {
            weapon.ChangePosition(bulletOriginsPositions[0].position);
            directionFacing = DIRECTION.UPLEFT;
            ChangeSprite(sprites[1], false);
        }
        else if ((dir.x < 0 && dir.y < 0) && directionFacing != DIRECTION.DOWNLEFT)
        {
            weapon.ChangePosition(bulletOriginsPositions[1].position);
            directionFacing = DIRECTION.DOWNLEFT;
            ChangeSprite(sprites[2], false);
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
