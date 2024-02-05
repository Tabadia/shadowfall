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

    public HealthHunger healthHunger;

    public bool dead = false;

    
    // Start is called before the first frame update
    //penis
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
            currentHunger = Mathf.Max(0, currentHunger - hungerRate);
            healthHunger.SetHunger(currentHunger);
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


}
