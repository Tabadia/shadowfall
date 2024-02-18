using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int maxHealth = 100;
    public int maxHunger = 100;

    public float currentHealth;
    public float currentHunger;

    public float stillHungerRate = 1f;
    public float walkHungerRate = 1.5f;
    public float sprintHungerRate = 2f;
    public float hungerRate = 1f;
    public float healthLostFromHungerRate = 2;
    public int timeAlive = 0;

    public HealthHunger healthHunger;

    public bool dead = false;

    
    // Start is called before the first frame update
    //penis
    void Start()
    {
        StartCoroutine(reduceHungerHealth());
        healthHunger.SetMaxHunger(maxHunger);
        currentHunger = maxHunger;
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if((currentHealth <= 0)){
            if (!dead){
                dead = true;
                print("You died");
            }
        }

        currentHunger = Mathf.Max(0,currentHunger - hungerRate/20);
        healthHunger.SetHunger(currentHunger);

        if (currentHealth > 0)
        {
            dead = false;
        }
    }

    IEnumerator reduceHungerHealth()
    {
        while (true){
            yield return new WaitForSeconds(1f);
            timeAlive++;
            //if (timeAlive % 5 == 0)
                //currentHunger = Mathf.Max(0, currentHunger - hungerRate);
            //healthHunger.SetHunger(currentHunger);

            if (currentHunger <= 0)
                currentHealth = Mathf.Max(0, currentHealth - healthLostFromHungerRate);
            healthHunger.SetHealth(currentHealth);
        }
    }

    public void SprintHunger(){
        hungerRate = sprintHungerRate;
    }

    public void WalkHunger(){
        hungerRate = walkHungerRate;
    }

    public void StillHunger(){
        hungerRate = stillHungerRate;
    }

    public void setHealth(float health)
    {
        currentHealth = health;
    }
    
    public void setHunger(float hunger)
    {
        currentHunger = hunger;
    }
}
