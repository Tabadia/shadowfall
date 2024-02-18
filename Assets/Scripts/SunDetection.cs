using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SunDetection : MonoBehaviour
{

    public Light directionalLight;
    public GameObject darkenScreen;
    public float maxDarkness;
    public float screenAlpha;

    public bool inSun = false;
    public bool inAnim = false;

    public Vector3 lightAngle;
    public Vector3 sunDir;

    public HealthHunger healthHunger;
    public Player player;
    public float sunDamage = 0.05f;

    private Vector3 topPos;
    private Vector3 centPos;
    //private Vector3 botPos;
    private Vector3 leftPos;
    private Vector3 rightPos;

    public GameObject topEffect;
    public GameObject botEffect;
    public GameObject leftEffect;
    public GameObject rightEffect;

    public bool canTakeDamage = true;
    float toRadians = (Mathf.PI/180);

    public GameObject pCamera;

    void Start() {
        // lightAngle = directionalLight.transform.forward * -1;
        // sunDir = directionalLight.transform.forward;
        maxDarkness = 0.5f;
    }

    void Update() {

        sunDir = directionalLight.transform.forward;
        lightAngle = directionalLight.transform.forward * -1;

        Vector2 latDir = new Vector2(sunDir.x, sunDir.z);
        float angle = Vector2.Angle(latDir, new Vector2(1, 0));
        
        //print(angle);

        centPos = new Vector3(pCamera.transform.position.x, pCamera.transform.position.y, pCamera.transform.position.z);
        topPos = new Vector3(centPos.x, centPos.y + (transform.localScale.y), centPos.z);
        //botPos = new Vector3(centPos.x, centPos.y - , centPos.z);
        leftPos = new Vector3(centPos.x - Mathf.Sin(angle * toRadians)*2f, centPos.y, centPos.z - Mathf.Cos(angle * toRadians)*2f);
        rightPos = new Vector3(centPos.x + Mathf.Sin(angle * toRadians)*2f, centPos.y, centPos.z + Mathf.Cos(angle * toRadians)*2f);

        
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
            Debug.DrawRay(topPos, lightAngle * 1000, Color.green);
            //Debug.DrawRay(botPos, lightAngle * 1000, Color.green);
            Debug.DrawRay(leftPos, lightAngle * 1000, Color.blue);
            Debug.DrawRay(rightPos, lightAngle * 1000, Color.blue);

            if (inSun == true && !inAnim){
                StartCoroutine(FadeIn());
            }
            if(hit.transform.tag != "Player"){
                inSun = false;
            }
            
            if (!Physics.Raycast(topPos, lightAngle, out hit, Mathf.Infinity, layerMask)){
                topEffect.SetActive(true);
            }
            else {
                topEffect.SetActive(false);
            }
            // if (!Physics.Raycast(botPos, lightAngle, out hit, Mathf.Infinity, layerMask)){
            //     botEffect.SetActive(true);
            // }
            // else {
            //     botEffect.SetActive(false);
            // }
            if (!Physics.Raycast(leftPos, lightAngle, out hit, Mathf.Infinity, layerMask)){
                leftEffect.SetActive(true);
            }
            else {
                leftEffect.SetActive(false);
            }
            if (!Physics.Raycast(rightPos, lightAngle, out hit, Mathf.Infinity, layerMask)){
                rightEffect.SetActive(true);
            }
            else {
                rightEffect.SetActive(false);
            }
        }
        else
        {
            Debug.DrawRay(centPos, lightAngle * 1000, Color.white);
            Debug.DrawRay(topPos, lightAngle * 1000, Color.green);
            //Debug.DrawRay(botPos, lightAngle * 1000, Color.green);
            Debug.DrawRay(leftPos, lightAngle * 1000, Color.blue);
            Debug.DrawRay(rightPos, lightAngle * 1000, Color.blue);

            if (inSun == false && !inAnim){
                StartCoroutine(FadeOut());
            }
            if(canTakeDamage & player.currentHealth > 0){
                StartCoroutine(reduceHealth());
            }
            inSun = true;     

            topEffect.SetActive(false);
            botEffect.SetActive(false);
            leftEffect.SetActive(false);
            rightEffect.SetActive(false);       
        }

    }

    IEnumerator FadeIn() {
        Image d = darkenScreen.GetComponent<Image>();
        Color dc = d.color;
        inAnim = true;
        while (dc.a < maxDarkness)
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
