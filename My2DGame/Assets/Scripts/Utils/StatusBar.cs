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
        if(character == null)
        {
            Debug.Log("character is null");
            return;
        }
        if (!character.isAlive)
        {
            Debug.Log("not alive");
            gameObject.SetActive(false);
            return;
        }

        if (!gameObject.activeSelf)
        {
            Debug.Log("alive");
            gameObject.SetActive(true);
        }

        float barFillValue = (float)character.currentHP / (float)character.maxHP;
        slider.value = barFillValue;

        if (slider.value <= slider.minValue)
        {
            Debug.Log("not enabled");
            barFillImage.enabled = false;
        }
        else
        {
            barFillImage.enabled = true;
        }
    }
}

