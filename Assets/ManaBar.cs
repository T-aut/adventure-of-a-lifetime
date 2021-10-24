using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManaBar : MonoBehaviour
{
    public Slider manaSlider;

    public void SetMana(float mana)
    {
        manaSlider.value = mana;
    }

    public void SetMaxMana(float maxMana)
    {
        manaSlider.maxValue = maxMana;
        manaSlider.value = maxMana;
    }

}
