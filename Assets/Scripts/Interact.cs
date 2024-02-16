using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Interact : MonoBehaviour
{
    public Transform interactCamera;
    public float playerActiveDistance;
    public GameObject sensedObject = null;
    public Player player;
    public string[] interactableObjects = {"canned_food", "energy_drink"};
    public TextMeshProUGUI interactText;
    public RectTransform crosshair;
    private Vector3 crosshairOrigin;
    private float timer = 0f;

    void Start() {
        crosshairOrigin = crosshair.anchoredPosition;
    }

    void Update() 
    {
        RaycastHit hit;
        interactText.enabled = false;
        if (Physics.Raycast(interactCamera.position, interactCamera.TransformDirection(Vector3.forward), out hit, playerActiveDistance)) 
        {
             sensedObject = hit.transform.gameObject;
        } else
        {
            sensedObject = null;
        }
        
        foreach (string name in interactableObjects)
        {
            if (sensedObject && sensedObject.name.Length >= name.Length && sensedObject.name.Substring(0,name.Length) == name)
            {
                timer = 0;
                while (timer < 1) {
                    timer += Time.deltaTime;
                    crosshair.localScale = Vector3.Lerp(new Vector3(.5f, .5f, .5f), new Vector3(1f, 1f, 1f), timer);
                    crosshair.anchoredPosition = Vector3.Lerp(crosshairOrigin, crosshairOrigin*2, timer);
                }
                interactText.enabled = true;
                if (Input.GetKeyDown(KeyCode.F))
                {
                    Invoke("Interact_" + name, 0);
                }
                break;
            }
            timer = 0;
            while (timer < 1) {
                timer += Time.deltaTime;
                crosshair.localScale = Vector3.Lerp(new Vector3(1f, 1f, 1f), new Vector3(.5f, .5f, .5f), timer);
                crosshair.anchoredPosition = Vector3.Lerp(crosshairOrigin*2, crosshairOrigin, timer);
            }
            /*
            new Vector2(crosshair.transform.position.x, Mathf.Lerp(crosshair.transform.position.y, targetY, Time.deltaTime));
            */
        }
    }
    
    void Interact_canned_food() 
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
        AudioSource source = GameObject.Find("canned_food_audio").GetComponent<AudioSource>();
        source.PlayOneShot(source.clip);
    }

    void Interact_energy_drink()
    {
        Debug.Log("yummy: " + sensedObject.name);
        DestroyImmediate(sensedObject.gameObject);
        sensedObject = null;
        player.GetComponent<PlayerController>().startSpeedBoost();
        AudioSource source = GameObject.Find("energy_drink_audio").GetComponent<AudioSource>();
        source.PlayOneShot(source.clip);
    }
}
