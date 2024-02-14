using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interact : MonoBehaviour
{
    public Transform camera;
    public float playerActiveDistance;
    bool ray = false;
    public GameObject sensedObject = null;
    public Player player;

    void Update() 
    {
        RaycastHit hit;
        if (Physics.Raycast(camera.position, camera.TransformDirection(Vector3.forward), out hit, playerActiveDistance)) 
        {
             sensedObject = hit.transform.gameObject;
        } else
        {
            sensedObject = null;
        }
        
        /* just add differnt "if" statements here if you want to interact with another object :) */

        if (Input.GetKeyDown(KeyCode.F) && sensedObject && sensedObject.name.Substring(0,11) == "canned_food")
        {
            float food_value = 50;

            Debug.Log("yummy: " + sensedObject.name);

            DestroyImmediate(sensedObject.gameObject);
            sensedObject = null;


            if (player.currentHealth <= 50)
            {
                player.setHealth(player.currentHealth+food_value);
                player.healthHunger.SetHealth(player.currentHealth);
            } else 
            {
                player.setHealth(player.maxHealth);
                player.healthHunger.SetHealth(player.maxHealth);
            }
            player.setHunger(player.maxHunger);
            player.healthHunger.SetHunger(player.maxHunger);    
        }

        if (Input.GetKeyDown(KeyCode.F) && sensedObject && sensedObject.name.Substring(0,12) == "energy_drink")
        {
            float food_value = 5;

            Debug.Log("yummy: " + sensedObject.name);

            DestroyImmediate(sensedObject.gameObject);
            sensedObject = null;

            player.GetComponent<PlayerController>().startSpeedBoost();
        }
    }
}
