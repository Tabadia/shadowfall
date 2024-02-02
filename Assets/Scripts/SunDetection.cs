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
    public bool inAnim = false;

    public Vector3 lightAngle;
    public Vector3 sunDir;

    public HealthHunger healthHunger;
    public Player player;
    public float sunDamage = 0.25f;

    private Vector3 topPos;
    private Vector3 centPos;
    private Vector3 botPos;
    private Vector3 leftPos;
    private Vector3 rightPos;

    public bool canTakeDamage = true;

    void Start() {
        // lightAngle = directionalLight.transform.forward * -1;
        // sunDir = directionalLight.transform.forward;
    }

    void Update() {

        sunDir = directionalLight.transform.forward;
        lightAngle = directionalLight.transform.forward * -1;

        Vector2 latDir = new Vector2(sunDir.x, sunDir.z);
        float angle = Vector2.Angle(latDir, new Vector2(1, 0));
        float toRadians = (Mathf.PI/180);
        print(angle);


        centPos = new Vector3(transform.position.x, transform.position.y + 1, transform.position.z);
        topPos = new Vector3(transform.position.x, transform.position.y + 2, transform.position.z);
        botPos = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        leftPos = new Vector3(transform.position.x - Mathf.Sin(angle * toRadians), transform.position.y, transform.position.z - Mathf.Cos(angle * toRadians));
        rightPos = new Vector3(transform.position.x + Mathf.Sin(angle * toRadians), transform.position.y, transform.position.z + Mathf.Cos(angle * toRadians));

        
        screenAlpha = darkenScreen.GetComponent<Image>().color.a;
        int layerMask = 1 << 8;

        // This would cast rays only against colliders in layer 8.
        // But instead we want to collide against everything except layer 8. The ~ operator does this, it inverts a bitmask.
        layerMask = ~layerMask;

        RaycastHit hit;
        // Does the ray intersect any objects excluding the player layer
        if (Physics.Raycast(centPos, lightAngle, out hit, Mathf.Infinity, layerMask))
        {
            Debug.DrawRay(centPos, lightAngle * hit.distance, Color.yellow);
            Debug.DrawRay(topPos, lightAngle * hit.distance, Color.yellow);
            Debug.DrawRay(botPos, lightAngle * hit.distance, Color.yellow);
            Debug.DrawRay(leftPos, lightAngle * hit.distance, Color.yellow);
            Debug.DrawRay(rightPos, lightAngle * hit.distance, Color.yellow);

            if (inSun == true && !inAnim){
                StartCoroutine(FadeIn());
            }
            if(hit.transform.tag != "Player"){
                inSun = false;
            }
            
        }
        else
        {
            Debug.DrawRay(centPos, lightAngle * 1000, Color.white);
            Debug.DrawRay(topPos, lightAngle * 1000, Color.green);
            Debug.DrawRay(botPos, lightAngle * 1000, Color.green);
            Debug.DrawRay(leftPos, lightAngle * 1000, Color.blue);
            Debug.DrawRay(rightPos, lightAngle * 1000, Color.blue);
            if (inSun == false && !inAnim){
                StartCoroutine(FadeOut());
            }
            if(canTakeDamage){
                StartCoroutine(reduceHealth());
            }
            inSun = true;            
        }

    }

    IEnumerator FadeIn() {
        Image d = darkenScreen.GetComponent<Image>();
        Color dc = d.color;
        inAnim = true;
        while (dc.a < 0.5f)
        {
            dc.a += 0.05f;
            d.color = dc;
            yield return new WaitForSeconds(0.05f);
        }
        inAnim = false;
        if (inSun == true){
            StartCoroutine(FadeOut());
        }
        yield return null;
    }

    IEnumerator FadeOut() {
        Image d = darkenScreen.GetComponent<Image>();
        Color dc = d.color;
        inAnim = true;
        while (dc.a > 0.05f)
        {
            dc.a -= 0.05f;
            d.color = dc;
            yield return new WaitForSeconds(0.05f);
        }
        inAnim = false;
        if (inSun == false){
            StartCoroutine(FadeIn());
        }
        yield return null;
    }

    IEnumerator reduceHealth()
    {
        canTakeDamage = false;
        yield return new WaitForSeconds(.1f);
        player.currentHealth -= sunDamage;
        healthHunger.SetHealth(player.currentHealth );
        canTakeDamage = true;
    }

}
