using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    [SerializeField]
    private float maxMovementSpeed = 5f;
    private float currentMovementSpeed;

    private void Awake()
    {
        currentMovementSpeed = maxMovementSpeed;
    }

    /// <summary>
    /// Moves the character in a given direction.
    /// </summary>
    /// <param name="direction">The direction to move the character.</param>
    public void Move(Vector3 direction)
    {
        transform.position += direction * currentMovementSpeed * Time.deltaTime;
    }
}
