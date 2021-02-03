using UnityEngine;

[RequireComponent(typeof(CharacterMovement))]
public class PlayerInputController : MonoBehaviour
{
    [SerializeField] private float maxStamina = 10f;
                     private float currentStamina;
    [SerializeField] private float staminaDecreaseMultiplier = 1f;
    [SerializeField] private float staminaIncreaseMultiplier = 1f;

    [SerializeField] private float rechargeWaitTime = 2f;
                     private float currentRechargeTime = 0f;

    private CharacterMovement characterMovement;

    //Event for stamina changes.
    public delegate void OnStaminaChangeHandler(float v);
    public event OnStaminaChangeHandler OnStaminaChange;

    private void Awake()
    {
        characterMovement = GetComponent<CharacterMovement>();
        currentStamina = maxStamina;
    }

    private void Update()
    {
        //Get input from keyboard/controller, and move the player in that direction.
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        Vector3 dir = new Vector3(h, v);

        //If the player has the stamina to move, do so.
        if (currentStamina > 0)
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
                IncreaseStamina();
        }
    }

    /// <summary>
    /// Decreases the stamina over time and with the multiplier in mind.
    /// </summary>
    private void DecreaseStamina()
    {
        //print("Stamina = " + currentStamina);
        currentStamina -= Time.deltaTime * staminaDecreaseMultiplier;
        OnStaminaChange?.Invoke(currentStamina);
    }

    /// <summary>
    /// Increases the stamina over time and with the multiplier in mind.
    /// </summary>
    private void IncreaseStamina()
    {
        currentStamina += Time.deltaTime * staminaIncreaseMultiplier;
        OnStaminaChange?.Invoke(currentStamina);
    }
}
