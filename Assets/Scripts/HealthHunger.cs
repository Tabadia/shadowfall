using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthHunger : MonoBehaviour
{
    public Image hungerCircle;
    public Image healthCircle;
    public Image healthHeart;

    public float maxHunger;
    public float maxHealth;

    public void SetMaxHealth(float max) {
        maxHealth = max;
    }

    public void SetHealth(float val){
        healthCircle.fillAmount = val/maxHealth;
        healthCircle.color = new Color32(255, (byte)(255 * healthCircle.fillAmount), (byte)(255 * healthCircle.fillAmount), 250);
        healthHeart.color = new Color32(255, (byte)(255 * healthCircle.fillAmount), (byte)(255 * healthCircle.fillAmount), 255);
    }

    public void SetMaxHunger(float max){
        maxHunger = max;
    }

    public void SetHunger(float val){
        hungerCircle.fillAmount = val/maxHunger;
    }
}
