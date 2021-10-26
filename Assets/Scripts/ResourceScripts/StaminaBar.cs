using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaminaBar : MonoBehaviour
{
    public Slider staminaSlider;
    public Text staminaText;

    public void SetStamina(float stamina)
    {
        // Make sure that the resource amount is within bounds of the slider values.
        staminaSlider.value = Mathf.Clamp(stamina, staminaSlider.minValue, staminaSlider.maxValue);
        SetStaminaText();
    }

    public void SetMaxStamina(float maxStamina)
    {
        staminaSlider.maxValue = maxStamina;
        staminaSlider.value = maxStamina;
        SetStaminaText();
    }

    void SetStaminaText()
    {
        staminaText.text = staminaSlider.value.ToString("0") + "/" + staminaSlider.maxValue.ToString("0") + " Stamina";
    }

}
