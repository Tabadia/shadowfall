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
            Debug.Log("yummy: " + sensedObject.name);

            DestroyImmediate(sensedObject.gameObject);
            sensedObject = null;

            player.setHealth(100);
            player.setHunger(100);
            player.healthHunger.SetHealth(100);
            player.healthHunger.SetHunger(100);
        }
    }
}
