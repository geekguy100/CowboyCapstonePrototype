using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class CharacterMovement : MonoBehaviour
{
    [SerializeField]
    private float maxMovementSpeed = 5f;
    [SerializeField]
    private float currentMovementSpeed;
    private SpriteRenderer spr;

    //Has the character's movement been modified from its default value?
    private bool modifiedSpeed = false;
    public bool ModifiedSpeed { get { return modifiedSpeed; } }

    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        currentMovementSpeed = maxMovementSpeed;
        spr =GetComponent<SpriteRenderer>();
    }

    /// <summary>
    /// Moves the character in a given direction.
    /// </summary>
    /// <param name="direction">The direction to move the character.</param>
    public void Move(Vector2 direction)
    {
        rb.velocity = direction * currentMovementSpeed;

        UpdateSpriteFlip(direction.x);
    }

    /// <summary>
    /// Multiplies the currnet movement speed by the amount provided.
    /// </summary>
    /// <param name="amnt">The amount to multiply the current movement speed by.</param>
    public void MultiplyMovementSpeed(float amnt)
    {
        modifiedSpeed = true;
        currentMovementSpeed *= amnt;
    }

    /// <summary>
    /// Resets movement speed to the default speed.
    /// </summary>
    public void ResetMovementSpeed()
    {
        modifiedSpeed = false;
        currentMovementSpeed = maxMovementSpeed;
    }

    private void UpdateSpriteFlip(float x)
    {
        if(x > 0)
        {
            spr.flipX = true;
        }
        else
        {
            spr.flipX = false;
        }
    }
}
