using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    private Slider slider;

    private void Start()
    {
        slider = GetComponent<Slider>();
        SetSliderValue(50);
    }


    public void SetSliderValue(float value)
    {
        slider.value = value;
    }
}
