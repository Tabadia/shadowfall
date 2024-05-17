using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine.SceneManagement;
// using System;

public class Interact : MonoBehaviour
{
    public Transform interactCamera;
    public float playerActiveDistance;
    public GameObject sensedObject = null;
    public Player player;
    public string[] interactableObjects = { "SodaPop", "JarredPickles"};
    public TextMeshProUGUI interactText;
    public RectTransform crosshair;
    public ObjectSaveManager objSave;

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

    public bool hasFlashlight = true;
    public GameObject flashlight;
    
    public float lookTimer = 0f;

    public GameObject nest;

    private ShadowMonster shadowScript;

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
        lookTimer -= .5f * Time.deltaTime;
        if (lookTimer <= 0){
            lookTimer = 0;
        }

        if(Input.GetKeyDown(KeyCode.F) && hasFlashlight){
            flashlight.SetActive(!flashlight.activeSelf);
            AudioClip randomClip = switchSounds[Random.Range(0, switchSounds.Length)];
            lightSwitchAudio.clip = randomClip;
            lightSwitchAudio.Play();
        }
        if (hasFlashlight && flashlight.activeSelf)
        {
            // Get the rotation of the player's camera
            Quaternion cameraRotation = interactCamera.rotation;

            // Set the rotation of the flashlight to match the camera rotation
            flashlight.transform.rotation = cameraRotation;
            
            RaycastHit hit2;
            if (Physics.Raycast(flashlight.transform.position, flashlight.transform.forward, out hit2, flashlight.GetComponent<Light>().range))
            {
                GameObject hitObject = hit2.transform.gameObject;
                Vector3 flashlightDirection = flashlight.transform.forward;
                Vector3 raycastDirection = hit2.point - flashlight.transform.position;
                float angle = Vector3.Angle(flashlightDirection, raycastDirection);
                if (angle < 0.17f && angle >= 0)
                {
                    print("looking at guy.");
                    lookTimer += 1.5f * Time.deltaTime;
                    if (lookTimer >= 4f){
                        //freeze
                        lookTimer = 0;
                        StartCoroutine(affectMonster(hitObject));
                    }
                }
                // check vector angle

                // if looking for certain amt of time
                // freeze for 1s
                // run opposite direction for 
            }
        }

        RaycastHit hit;
        InteractUI(false);
        sensedObject = Physics.Raycast(interactCamera.position, interactCamera.TransformDirection(Vector3.forward), out hit, playerActiveDistance) 
            ? hit.transform.gameObject : null;
        if (sensedObject != null){
            if(sensedObject.tag == "Boardable"){
                InteractUI(true);
                if (Input.GetKeyDown(KeyCode.E) && !sensedObject.name.Contains("BOARDED")){ 
                    sensedObject.name += " BOARDED"; 
                    StartCoroutine(placeBoards(sensedObject));
                }  
            }
            if(sensedObject.tag == "Door"){
                InteractUI(true);
                if (Input.GetKeyDown(KeyCode.E))
                {
                    Animator animator = sensedObject.GetComponent<Animator>();
                    if (animator != null)
                    {
                        bool open = animator.GetBool("open");
                        if(open){
                            animator.Play("doorOpen", 0, 0.0f);
                            animator.SetBool("open", false);
                        }
                        else{
                            animator.Play("doorClose", 0, 0.0f);
                            animator.SetBool("open", true);
                        }
                    }
                }
            }
            if(sensedObject.tag == "Light"){
                InteractUI(true);
                if (Input.GetKeyDown(KeyCode.E)) {
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
                if (Input.GetKeyDown(KeyCode.E))
                {
                    var item = sensedObject.GetComponent<GroundItem>();
                    //item.item.obj = Instantiate(sensedObject);
                    if (player.inventory.AddItem(item.item, 1))
                    {
                        objSave.RemoveObject(sensedObject);
                        //item.item.obj.SetActive(false);
                    }
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

        // check if the player is clicking
        // if they are clicking check the static interface to see whaqt item is being held/selected 
        // make a variable 
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

    /*void Interact_    () 
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

    IEnumerator placeBoards(GameObject window)
    {
        // if player has planks in inventory
        for (int i = 0; i < 3; i++)
        {
            GameObject plank = Instantiate(planks[Random.Range(0, planks.Length)], window.transform);
            // plank.transform.SetParent(window.transform.parent.gameObject.transform);
            float randomZRotation = Random.Range(-10, 10);
            plank.transform.localPosition = new Vector3(0, 9/10 + (i * 2.5f)/10 -2.5f/10, -1);
            plank.transform.localRotation = Quaternion.Euler(0, 0, randomZRotation);
            // //plank.transform.localRotation = Quaternion.Euler(0, 0, 0);
            plank.transform.localScale = new Vector3(10, 10, 10);
            yield return new WaitForSeconds(.5f);
        }
    }

    IEnumerator affectMonster(GameObject monster)
    {
        GameObject monsterAnim = monster.transform.parent.gameObject;
        monster = monsterAnim.transform.parent.gameObject;
        shadowScript = monster.GetComponent<ShadowMonster>();
        print(monster.name);
        shadowScript.setChasingPlayer(false);
        monster.GetComponent<UnityEngine.AI.NavMeshAgent>().SetDestination(monster.transform.position);
        monsterAnim.GetComponent<Animator>().speed = 0;
        yield return new WaitForSeconds(1.5f);
        monsterAnim.GetComponent<Animator>().speed = 1;
        lookTimer = 0;
        if (Random.Range(0,2) == 0)
        {
            monsterAnim.GetComponent<Animator>().SetBool("isDead", true);
            print("monster dead");
            yield return new WaitForSeconds(10f);
            monster.SetActive(false);
        }
        else
        {
            monster.GetComponent<UnityEngine.AI.NavMeshAgent>().SetDestination(nest.transform.position);
            print("monster ran away");
        }
    }
}
