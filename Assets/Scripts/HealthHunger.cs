using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthHunger : MonoBehaviour
{
    public Image hungerCircle;
    public Image healthCircle;

    public float maxHunger;
    public float maxHealth;

    public void SetMaxHealth(float max) {
        maxHealth = max;
    }

    public void SetHealth(float val){
        healthCircle.fillAmount = val/maxHealth;
    }

    public void SetMaxHunger(float max){
        maxHunger = max;
    }

    public void SetHunger(float val){
        hungerCircle.fillAmount = val/maxHunger;
    }
}
