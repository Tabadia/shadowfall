using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using static UnityEditor.Progress;
// using System;

public class Interact : MonoBehaviour
{
    public Transform interactCamera;
    public float playerActiveDistance;
    public GameObject sensedObject = null;
    public Player player;
    public string[] interactableObjects = { "CocaCola", "CannedPeaches", "JarredPickles"};
    public TextMeshProUGUI interactText;
    public RectTransform crosshair;

    // public GameObject wall;
    public GameObject[] planks;

    // private Vector3 crosshairCurrentSize;
    private Vector3 crosshairGoalSizeSmall;
    private Vector3 crosshairGoalSizeBig;
    // private Vector2 crosshairCurrentPos;
    private Vector2 crosshairGoalPosSmall;
    private Vector2 crosshairGoalPosBig;

    public AudioSource lightSwitchAudio;
    public AudioClip[] switchSounds;
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
        RaycastHit hit;
        InteractUI(false);
        sensedObject = Physics.Raycast(interactCamera.position, interactCamera.TransformDirection(Vector3.forward), out hit, playerActiveDistance) 
            ? hit.transform.gameObject : null;
        if (sensedObject != null){
            if(sensedObject.tag == "Boardable"){
                InteractUI(true);
                if (Input.GetKeyDown(KeyCode.F) && !sensedObject.name.Contains("BOARDED")){ 
                    sensedObject.name += " BOARDED"; 
                    StartCoroutine(placeBoards(sensedObject));
                }  
            }
            if(sensedObject.tag == "Light"){
                InteractUI(true);
                if (Input.GetKeyDown(KeyCode.F)) {
                    foreach (Transform child in sensedObject.GetComponentsInChildren<Transform>(true))
                    {
                        Light lightComponent = child.GetComponent<Light>();
                        if (lightComponent != null)
                        {
                            child.gameObject.SetActive(!child.gameObject.activeSelf);
                                AudioClip randomClip = switchSounds[Random.Range(0, switchSounds.Length)];
                                lightSwitchAudio.clip = randomClip;
                                lightSwitchAudio.Play();
                            break;
                        }
                    }
                }
            }
            if (sensedObject.tag == "GroundItem")
            {
                InteractUI(true);
                if (Input.GetKeyDown(KeyCode.F))
                {
                    var item = sensedObject.GetComponent<GroundItem>();
                    if (player.inventory.AddItem(item.item, 1))
                        Destroy(sensedObject.gameObject);
                    sensedObject = null;

                }
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
    public void InteractUI(bool isActive)
    {
        if (isActive)
        {
            crosshair.anchoredPosition = crosshairGoalPosBig;
            crosshair.localScale = crosshairGoalSizeBig;
            interactText.enabled = true;
            return;
        }
        crosshair.anchoredPosition = crosshairGoalPosSmall;
        crosshair.localScale = crosshairGoalSizeSmall;
        interactText.enabled = false;
    }
    /*void Interact_CannedPeaches() 
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
    }*/

    IEnumerator placeBoards(GameObject wall)
    {
        // if player has planks in inventory
        for (int i = 0; i < 3; i++)
        {
            GameObject plank = Instantiate(planks[Random.Range(0, planks.Length)]);
            plank.transform.SetParent(wall.transform.parent.gameObject.transform);
            plank.transform.localPosition = new Vector3(0, 9 + (i * 2.5f), -1);
            plank.transform.localScale = new Vector3(100, 100, 100);
            float randomZRotation = Random.Range(-10, 10);
            plank.transform.localRotation = Quaternion.Euler(0, 0, randomZRotation);
            //plank.transform.localRotation = Quaternion.Euler(0, 0, 0);
            yield return new WaitForSeconds(.5f);
        }
    }
}
