using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManaBar : MonoBehaviour
{
    public Slider manaSlider;
    public Text manaText;

    public void SetMana(float mana)
    {
        // Make sure that the resource amount is within bounds of the slider values.
        manaSlider.value = Mathf.Clamp(mana, manaSlider.minValue, manaSlider.maxValue);
        SetManaText();
    }

    public void SetMaxMana(float maxMana)
    {
        manaSlider.maxValue = maxMana;
        manaSlider.value = maxMana;
        SetManaText();
    }

    void SetManaText()
    {
        manaText.text = manaSlider.value.ToString("0") + "/" + manaSlider.maxValue.ToString("0") + " Mana";
    }

}
