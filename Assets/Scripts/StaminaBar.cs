using UnityEngine;
using UnityEngine.UI;

public class StaminaBar : MonoBehaviour
{
    private Slider slider;

    private PlayerInputController playerInput;

    private void Awake()
    {
        slider = GetComponent<Slider>();
        slider.value = slider.maxValue;

        //Get PlayerInputController and subscribe to the OnStaminaChange event.
        playerInput = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInputController>();
        playerInput.OnStaminaChange += ChangeValue;
    }

    private void OnDestroy()
    {
        playerInput.OnStaminaChange -= ChangeValue;
    }

    /// <summary>
    /// Change the stamina slider's value.
    /// </summary>
    /// <param name="v">The stamina's value.</param>
    /// <param name="v">The max stamina.</param>
    public void ChangeValue(float v, float maxV)
    {
        if (slider.maxValue != maxV)
            slider.maxValue = maxV;

        slider.value = v;
    }
}
