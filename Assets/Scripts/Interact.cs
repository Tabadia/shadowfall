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
    public string[] interactableObjects = {"energy_drink", "CannedPeaches"};
    public TextMeshProUGUI interactText;
    public RectTransform crosshair;
    // private Vector3 crosshairCurrentSize;
    private Vector3 crosshairGoalSizeSmall;
    private Vector3 crosshairGoalSizeBig;
    // private Vector2 crosshairCurrentPos;
    private Vector2 crosshairGoalPosSmall;
    private Vector2 crosshairGoalPosBig;
    // private float duration = 10;
    // public float time = 0;    

    void Start() {
        // crosshairCurrentSize = crosshair.localScale;
        crosshairGoalSizeSmall = crosshair.localScale;
        crosshairGoalSizeBig = crosshair.localScale*2;
        // crosshairCurrentPos = crosshair.anchoredPosition;
        crosshairGoalPosSmall = crosshair.anchoredPosition;
        crosshairGoalPosBig = crosshair.anchoredPosition*2;
    }

    void Update() 
    {
        crosshair.anchoredPosition = crosshairGoalPosSmall;
        crosshair.localScale = crosshairGoalSizeSmall;
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
                crosshair.anchoredPosition = crosshairGoalPosBig;
                crosshair.localScale = crosshairGoalSizeBig;
                interactText.enabled = true;
                if (Input.GetKeyDown(KeyCode.F))
                {  
                    player.inventory.AddItem(sensedObject.GetComponent<GroundItem>().item, 1);
                    DestroyImmediate(sensedObject.gameObject);
                    sensedObject = null;
                    
                }
                break;
            }
        }
        if (Input.GetKeyDown(KeyCode.Keypad8))
        {
            player.inventory.Save();
        }
        if (Input.GetKeyDown(KeyCode.Keypad9))
        {
            player.inventory.Load();
        }
    }
    // IEnumerator crosshairLerping()
    // {
    //     Debug.Log("working???");
    //     while (true) {
    //         // yield return new WaitForSeconds(.1f);
    //         // time = 0;
    //         // bool lerped = false;
    //         // crosshairCurrentSize = crosshair.localScale;
    //         // crosshairCurrentPos = crosshair.anchoredPosition;
    //         // foreach (string name in interactableObjects)
    //         // {
    //         //     if (sensedObject && sensedObject.name.Length >= name.Length && sensedObject.name.Substring(0,name.Length) == name)
    //         //     {
    //         //         lerped = true;
    //         //         while (time < duration) {
    //         //             crosshair.localScale = Vector3.Lerp(crosshairCurrentSize, crosshairGoalSizeBig, time / duration);
    //         //             crosshair.anchoredPosition = Vector2.Lerp(crosshairCurrentPos, crosshairGoalPosBig, time / duration);
    //         //             time += Time.deltaTime;
    //         //         }
    //         //     }
    //         //     else if (!lerped)
    //         //     {
    //         //         while (time < duration) {
    //         //             crosshair.localScale = Vector3.Lerp(crosshairCurrentSize, crosshairGoalSizeSmall, time / duration);
    //         //             crosshair.anchoredPosition = Vector2.Lerp(crosshairCurrentPos, crosshairGoalPosSmall, time / duration);
            //             time += Time.deltaTime;
            //         }
    //         //     }
    //         // }
    //     }
    // }
    
    void Interact_CannedPeaches() 
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
