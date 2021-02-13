using UnityEngine;

[RequireComponent(typeof(CharacterMovement))]
public class PlayerInputController : MonoBehaviour
{
    [Header("Stamina")]
    [SerializeField] private float maxStamina = 10f;
                     private float currentStamina;
    [SerializeField] private float staminaDecreaseMultiplier = 1f; //How much faster stamina should decrease.
    [SerializeField] private float staminaIncreaseMultiplier = 1f; //How much faster stamina should increase.
    [SerializeField] private float movementReductionMultiplier = 0.5f; //How much slower the player moves when out of stamina.

    [SerializeField] private float rechargeWaitTime = 2f;          //How long the player must wait until stamina begins recharging.
                     private float currentRechargeTime = 0f;       //The amount of time the player has spent waiting for stamina to begin recharging.

    private CharacterMovement characterMovement;

    //Event for stamina changes.
    public delegate void OnStaminaChangeHandler(float v, float mv);
    public event OnStaminaChangeHandler OnStaminaChange;

    private void Awake()
    {
        characterMovement = GetComponent<CharacterMovement>();
        currentStamina = maxStamina;
    }

    //Moving the character in FixedUpdate since it relies on Rigidbody2D.MovePosition().
    private void FixedUpdate()
    {
        //Get input axes, and move the player in that direction.
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        Vector3 dir = new Vector3(h, v);

        //Player will walk slower when out of stamina.
        if (currentStamina <= 0 && !characterMovement.ModifiedSpeed)
            characterMovement.MultiplyMovementSpeed(movementReductionMultiplier);  

        //Apply movement.
        characterMovement.Move(dir);


        HandleStaminaChanges(dir);
    }

    /// <summary>
    /// Handles managing player's stamina.
    /// </summary>
    /// <param name="dir">Player's movement input from Input.GetAxis("").</param>
    private void HandleStaminaChanges(Vector3 dir)
    {
        //If the player is moving, decrease stamina.
        //If they are standing still, increase stamina.
        if (currentStamina > 0 && dir != Vector3.zero)
        {
            //Reset recharge timer.
            if (currentRechargeTime > 0)
                currentRechargeTime = 0;

            DecreaseStamina();
        }
        else if (currentStamina < maxStamina && dir == Vector3.zero) //Player can recharge stamina since they're standing still.
        {
            //If the recharge time is not maxed, keep waiting.
            if (currentRechargeTime < rechargeWaitTime)
                currentRechargeTime += Time.deltaTime;
            else //Once the recharge time is maxed, start increasing stamina.
            {
                IncreaseStamina();

                //If the character is still moving at a modified speed, reset it to default speed.
                //Since they have stamina, they can now move normally.
                if (characterMovement.ModifiedSpeed)
                    characterMovement.ResetMovementSpeed();
            }
        }
    }

    /// <summary>
    /// Decreases the stamina over time and with the multiplier in mind.
    /// </summary>
    private void DecreaseStamina()
    {
        //print("Stamina = " + currentStamina);
        currentStamina -= Time.deltaTime * staminaDecreaseMultiplier;
        OnStaminaChange?.Invoke(currentStamina, maxStamina);
    }

    /// <summary>
    /// Increases the stamina over time and with the multiplier in mind.
    /// </summary>
    private void IncreaseStamina()
    {
        currentStamina += Time.deltaTime * staminaIncreaseMultiplier;
        OnStaminaChange?.Invoke(currentStamina, maxStamina);
    }
}
