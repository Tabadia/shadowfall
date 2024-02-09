using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interact : MonoBehaviour
{
    public Transform camera;
    public float playerActiveDistance;
    bool active = false;

    void Update() 
    {
        RaycastHit hit;
        active = Physics.Raycast(camera.position, camera.TransformDirection(Vector3.forward), out hit, playerActiveDistance);
        if (Input.GetKeyDown(KeyCode.F) && active == true) 
        {
            // if (hit.gameObject.GetComponent<Name>() == "canned_food") 
            // {
            //     Debug.Log("eat");
            // }
        }
    }
}
