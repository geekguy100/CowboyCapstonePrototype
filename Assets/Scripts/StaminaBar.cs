using UnityEngine;
using UnityEngine.UI;

public class StaminaBar : MonoBehaviour
{
    private Slider slider;

    private PlayerInputController playerInput;

    [Tooltip("Text that displays 'low energy' when the player is low on stamina.")]
    [SerializeField] private GameObject lowEnergyText;

    // How much stamina the player needs until low-energy pops up.
    private float lowEnergy = 3;

    private void Awake()
    {
        slider = GetComponent<Slider>();
        slider.value = slider.maxValue;

        //Get PlayerInputController and subscribe to the OnStaminaChange event.
        playerInput = GameObject.Find("Player").GetComponent<PlayerInputController>();
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
        if (v <= lowEnergy && !lowEnergyText.activeSelf)
            lowEnergyText.SetActive(true);
        else if (v > lowEnergy && lowEnergyText.activeSelf)
            lowEnergyText.SetActive(false);
    }
}
