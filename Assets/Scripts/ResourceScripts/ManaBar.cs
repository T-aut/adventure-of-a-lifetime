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
        manaSlider.value = mana;
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
