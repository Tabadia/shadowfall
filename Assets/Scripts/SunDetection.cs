using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunDetection : MonoBehaviour
{

    public Light directionalLight;
    public bool inSun = false;
    public Vector3 lightAngle;

    // Start is called before the first frame update
    void Start()
    {
        lightAngle = directionalLight.transform.eulerAngles;
        print(lightAngle.x + " " + lightAngle.y + " " + lightAngle.z);
        lightAngle.x = -360;
    }

    // Update is called once per frame
    void Update()
    {
        int layerMask = 1 << 8;

        // This would cast rays only against colliders in layer 8.
        // But instead we want to collide against everything except layer 8. The ~ operator does this, it inverts a bitmask.
        layerMask = ~layerMask;

        RaycastHit hit;
        // Does the ray intersect any objects excluding the player layer
        if (Physics.Raycast(transform.position, lightAngle, out hit, Mathf.Infinity, layerMask))
        {
            Debug.DrawRay(transform.position, lightAngle * hit.distance, Color.yellow);
            inSun = false;
        }
        else
        {
            Debug.DrawRay(transform.position, lightAngle * 1000, Color.white);
            inSun = true;
        }
    }
}
