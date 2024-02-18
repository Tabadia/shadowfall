using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthHunger : MonoBehaviour
{
    public Slider healthSlider;
    public Image hungerCircle;

    public float maxHunger;

    public void SetMaxHealth(float max){
        healthSlider.maxValue = max;
        healthSlider.value = max;
    }

    public void SetHealth(float val){
        healthSlider.value = val;
    }

    public void SetMaxHunger(float max){
        maxHunger = max;
        
    }

    public void SetHunger(float val){
        hungerCircle.fillAmount = val/maxHunger;
    }
}
