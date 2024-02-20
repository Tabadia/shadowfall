using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public InventoryObject inventory;
    
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
    
    private void OnApplicationQuit()
    {
        inventory.Containter.Clear();
    }
    
    void Start()
    {
        healthHunger.SetMaxHunger(maxHunger);
        healthHunger.SetMaxHealth(maxHealth);
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


        if (currentHealth > 0)
        {
            dead = false;
        }

            currentHunger = Mathf.Max(0, currentHunger - hungerRate * Time.deltaTime);
            healthHunger.SetHunger(currentHunger);
            currentHealth = Mathf.Max(0, currentHealth - healthLostFromHungerRate * Time.deltaTime);
            healthHunger.SetHealth(currentHealth);
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
