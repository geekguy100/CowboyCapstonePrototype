using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class CharacterMovement : MonoBehaviour
{
    [SerializeField] private float maxMovementSpeed = 5f;
    [SerializeField] private float currentMovementSpeed;

    // Not needed, but allows a character to flip.
    [SerializeField] private CharacterFlipper flipper;

    //Has the character's movement been modified from its default value?
    private bool modifiedSpeed = false;
    public bool ModifiedSpeed { get { return modifiedSpeed; } }

    private Rigidbody2D rb;

    private void Awake()
    {
        currentMovementSpeed = maxMovementSpeed;

        rb = GetComponent<Rigidbody2D>();
    }

    /// <summary>
    /// Moves the character in a given direction.
    /// </summary>
    /// <param name="direction">The direction to move the character.</param>
    public void Move(Vector2 direction)
    {
        rb.velocity = direction * currentMovementSpeed;

        if (flipper != null)
            flipper.CheckDirection(direction);
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
}
