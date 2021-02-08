using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class CharacterMovement : MonoBehaviour
{
    [SerializeField]
    private float maxMovementSpeed = 5f;
    [SerializeField]
    private float currentMovementSpeed;

    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        currentMovementSpeed = maxMovementSpeed;
    }

    /// <summary>
    /// Moves the character in a given direction.
    /// </summary>
    /// <param name="direction">The direction to move the character.</param>
    public void Move(Vector2 direction)
    {
        rb.velocity = direction * currentMovementSpeed;
    }
}
