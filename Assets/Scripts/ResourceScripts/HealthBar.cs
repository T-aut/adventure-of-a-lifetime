using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider healthSlider;
    public Text healthText;

    public void SetHealth(float health)
    {
        // Make sure that the resource amount is within bounds of the slider values.
        healthSlider.value = Mathf.Clamp(health, healthSlider.minValue, healthSlider.maxValue);
        SetHealthText();
    }

    public void SetMaxHealth(float maxHealth)
    {
        healthSlider.maxValue = maxHealth;
        healthSlider.value = maxHealth;
        SetHealthText();
    }

    void SetHealthText()
    {
        healthText.text = healthSlider.value.ToString("0") + "/" + healthSlider.maxValue.ToString("0") + " Health";
    }

}
