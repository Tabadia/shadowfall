using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int maxHealth = 100;
    public int maxHunger = 100;

    public float currentHealth;
    public float currentHunger;

    public float hungerRate = 2f;

    public HealthHunger healthHunger;

    public bool dead = false;

    
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(reduceHunger());
        currentHunger = maxHunger;
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if((currentHealth <= 0) ||(currentHunger <= 0)){
            if (!dead){
                dead = true;
                print("You died");
            }
        }
    }

    IEnumerator reduceHunger()
    {
        while (true){
            yield return new WaitForSeconds(5f);
            currentHunger -= hungerRate;
            healthHunger.SetHunger(currentHunger);
        }

    }
}
