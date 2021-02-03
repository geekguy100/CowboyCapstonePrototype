using UnityEngine;

[RequireComponent(typeof(CharacterMovement))]
public class PlayerInputController : MonoBehaviour
{
    [Header("Stamina")]
    [SerializeField] private float maxStamina = 10f;
                     private float currentStamina;
    [SerializeField] private float staminaDecreaseMultiplier = 5f;

    private CharacterMovement characterMovement;

    public delegate void OnStaminaChangeHandler();
    public event OnStaminaChangeHandler OnStaminaChange;

    private void Awake()
    {
        characterMovement = GetComponent<CharacterMovement>();
    }

    private void Update()
    {
        //Get input from keyboard/controller, and move the player in that direction.
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        Vector3 dir = new Vector3(h, v);

        //If the player is moving, decrease stamina.
        if (dir != Vector3.zero)
            DecreaseStamina();

        //If the player has the stamina to move, do so.
        if (currentStamina > 0)
            characterMovement.Move(dir);
    }

    /// <summary>
    /// Decreases the stamina over time and with the multiplier in mind.
    /// </summary>
    private void DecreaseStamina()
    {
        currentStamina -= Time.deltaTime * staminaDecreaseMultiplier;
        OnStaminaChange?.Invoke();
    }
}
