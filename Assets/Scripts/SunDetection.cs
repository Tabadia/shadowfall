using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SunDetection : MonoBehaviour
{

    public Light directionalLight;
    public GameObject darkenScreen;
    public float screenAlpha;
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
        screenAlpha = darkenScreen.GetComponent<Image>().color.a;
        int layerMask = 1 << 8;

        // This would cast rays only against colliders in layer 8.
        // But instead we want to collide against everything except layer 8. The ~ operator does this, it inverts a bitmask.
        layerMask = ~layerMask;

        RaycastHit hit;
        // Does the ray intersect any objects excluding the player layer
        if (Physics.Raycast(transform.position, lightAngle, out hit, Mathf.Infinity, layerMask))
        {
            Debug.DrawRay(transform.position, lightAngle * hit.distance, Color.yellow);
            
            if (inSun == true){
                StartCoroutine(FadeIn());
            }
            inSun = false;
            
        }
        else
        {
            Debug.DrawRay(transform.position, lightAngle * 1000, Color.white);
            
            if (inSun == false){
                StartCoroutine(FadeOut());
            }
            inSun = true;
        }
    }

    IEnumerator FadeIn()
    {
        Image d = darkenScreen.GetComponent<Image>();
        Color dc = d.color;
        while (dc.a < 0.5f)
        {
            dc.a += 0.05f;
            d.color = dc;
            yield return new WaitForSeconds(0.05f);
        }
        yield return null;
    }

    IEnumerator FadeOut()
    {
        Image d = darkenScreen.GetComponent<Image>();
        Color dc = d.color;
        while (dc.a > 0.05f)
        {
            dc.a -= 0.05f;
            d.color = dc;
            yield return new WaitForSeconds(0.05f);
        }
        yield return null;
    }
}
