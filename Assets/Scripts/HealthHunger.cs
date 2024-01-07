using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthHunger : MonoBehaviour
{
    public Slider healthSlider;
    public Slider hungerSlider;

    public void SetMaxHealth(float max){
        healthSlider.maxValue = max;
        healthSlider.value = max;
    }

    public void SetHealth(float val){
        healthSlider.value = val;
    }

    public void SetMaxHunger(float max){
        hungerSlider.maxValue = max;
        hungerSlider.value = max;
    }

    public void SetHunger(float val){
        hungerSlider.value = val;
    }
}
