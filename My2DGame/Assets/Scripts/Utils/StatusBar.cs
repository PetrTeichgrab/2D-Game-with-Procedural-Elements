using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class StatusBar : MonoBehaviour
{
    public Character character;
    public Image barFillImage;
    private Slider slider;
    void Start()
    {
        slider = GetComponent<Slider>();
    }

    void Update()
    {
        float barFillValue = (float)character.currentHP / (float)character.maxHP;
        slider.value = barFillValue;
        if (slider.value <= slider.minValue)
        {
            barFillImage.enabled = false;
        }
        if (slider.value >= slider.minValue && barFillImage.enabled) {
            barFillImage.enabled = true;
        }
    }
}
