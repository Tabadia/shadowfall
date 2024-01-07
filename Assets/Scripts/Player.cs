using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int maxHealth = 100;
    public int maxHunger = 100;

    public int currentHealth;
    public int currentHunger;

    public float hungerRate = 0.02f;

    public HealthHunger healthHunger;

    
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(reduceHunger());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator reduceHunger()
    {
        while (true){
            yield return new WaitForSeconds(5f);
            healthHunger.SetHunger(healthHunger.hungerSlider.value - hungerRate);
            print("reducing!!!");
        }

    }
}
