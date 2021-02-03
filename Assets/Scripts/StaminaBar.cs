using UnityEngine;
using UnityEngine.UI;

public class StaminaBar : MonoBehaviour
{
    private Slider slider;
    private float maxValue;

    private void Awake()
    {
        slider = GetComponent<Slider>();
        slider.maxValue = maxValue;

        //Get PlayerInputController and subscribe to the OnStaminaChange event.
    }

    /// <summary>
    /// Change the slider's 
    /// </summary>
    /// <param name="v"></param>
    private void ChangeValue(float v)
    {
        slider.value = v;
    }
}
