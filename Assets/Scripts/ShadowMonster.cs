using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using System.Collections.Generic;

public class ShadowMonster : MonoBehaviour
{
    private GameObject player; // Reference to the player GameObject
    private NavMeshAgent navMeshAgent;
    public float detectionRadius = 1.5f; // Adjust the detection radius as needed
    public float stoppingDistanceOffset = 0.5f; // Offset to stop before reaching the player
    public float lightDetectionRadius = 5f; // Radius to detect lights
    public float flickerDuration = 0.05f; // Duration of each flicker
    private bool flickerCooldown = false; // Cooldown flag for flickering lights
    private List<GameObject> flickeringLights = new List<GameObject>(); // List of lights being flickered
    public AudioSource flickerAudioSource; // Public variable for the flicker sound effect audio source
    public AudioClip[] flickerSounds; // Array of flicker sound effects
    public AudioClip[] monsterSounds; // Array of monster sound effects
    private Dictionary<GameObject, float> flickerCooldowns = new Dictionary<GameObject, float>(); // Dictionary to track cooldown times for lights
    public float normalSpeed = 3.5f; // Normal movement speed
    public float slowSpeed = 1.5f; // Slower movement speed within the light detection radius
    public float turnOffLightsRadius = 3f; // Radius to turn off lights
    public float damageAmount = 10f; // Amount of damage inflicted on the player
    public float damageCooldown = 2f; // Cooldown time between damage ticks
    private float lastMonsterSoundTime = 0f; // Timestamp of the last monster sound played
    public float monsterSoundCooldown = 5f; // Cooldown time between monster sounds
    private float lastDamageTime = 0f; // Timestamp of the last damage
    public GameObject nest;
    public GameObject childMonster;
    public bool chasingPlayer = true;
    private Animator animator;

    void Start()
    {
        animator = childMonster.GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player");
        navMeshAgent = GetComponent<NavMeshAgent>();
        if (player == null)
        {
            Debug.LogError("Player GameObject not found with tag 'Player'!");
        }

        // Set the NavMeshAgent's stopping distance based on the detection radius
        navMeshAgent.stoppingDistance = detectionRadius + stoppingDistanceOffset;
        navMeshAgent.speed = normalSpeed; // Set initial movement speed
    }

    void Update()
    {

        if (navMeshAgent.velocity.magnitude > 0)
        {
            animator.SetBool("isWalking", true);
        }
        else
        {
            animator.SetBool("isWalking", false);
        }

        if (player != null && chasingPlayer)
        {
            navMeshAgent.SetDestination(player.transform.position);

            // Check if the player is nearby using an overlap sphere
            Collider[] colliders = Physics.OverlapSphere(transform.position, detectionRadius);
            foreach (Collider collider in colliders)
            {
                if (collider.CompareTag("Player"))
                {
                    Debug.Log("Player detected nearby!");
                    // Add actions here, such as triggering an alert or changing behavior
                    DamagePlayer();
                }
            }

            // Check for lights using a larger overlap sphere
            Collider[] lightColliders = Physics.OverlapSphere(transform.position, lightDetectionRadius);
            foreach (Collider collider in lightColliders)
            {
                if (collider.CompareTag("Light"))
                {
                    // Check if the light is not already flickering and not on cooldown
                    if (!flickeringLights.Contains(collider.gameObject) && !flickerCooldown && !IsOnCooldown(collider.gameObject))
                    {
                        StartCoroutine(FlickerLight(collider.gameObject));
                    }
                }
                if (collider.CompareTag("Player"))
                {
                    PlayRandomMonsterSound();
                }
            }

            // Check for lights to turn off within the turnOffLightsRadius
            Collider[] turnOffLightsColliders = Physics.OverlapSphere(transform.position, turnOffLightsRadius);
            foreach (Collider collider in turnOffLightsColliders)
            {
                if (collider.CompareTag("Light"))
                {
                    Light lightComponent = collider.GetComponentInChildren<Light>();
                    if (lightComponent != null)
                    {
                        lightComponent.enabled = false; // Turn off the light
                    }
                }
            }

            // Adjust movement speed based on lights in collision
            bool lightsInCollision = false;
            foreach (Collider collider in colliders)
            {
                if (collider.CompareTag("Light"))
                {
                    lightsInCollision = true;
                    break;
                }
            }

            if (lightsInCollision)
            {
                navMeshAgent.speed = slowSpeed; // Set slower movement speed
            }
            else
            {
                navMeshAgent.speed = normalSpeed; // Set normal movement speed
            }
        }
    }

    void DamagePlayer()
    {
        if (Time.time - lastDamageTime >= damageCooldown)
        {
            animator.SetTrigger("Attack");
            Player playerScript = player.GetComponent<Player>();
            if (playerScript != null)
            {
                playerScript.setHealth(playerScript.currentHealth - damageAmount);
                lastDamageTime = Time.time;
            }
        }
    }

    IEnumerator FlickerLight(GameObject lightObject)
    {
        flickeringLights.Add(lightObject); // Add the light to the flickering list
        flickerCooldown = true; // Set the global flicker cooldown

        Light lightComponent = lightObject.GetComponentInChildren<Light>(); // Get the light component directly
        if (lightComponent != null)
        {
            // Store the initial state of the light
            bool initialState = lightComponent.enabled;

            PlayRandomFlickerSound(); // Play the flicker sound effect
            // Flicker the light by turning it on and off quickly
            for (int i = 0; i < 10; i++) // Flicker 10 times quickly
            {
                lightComponent.enabled = !lightComponent.enabled; // Toggle the light state
                Debug.Log("Flicker occurred!"); // Print when flicker occurs
                yield return new WaitForSeconds(flickerDuration); // Wait for the flicker duration
            }

            // Restore the light to its initial state
            lightComponent.enabled = initialState;
            // Set the cooldown time for this light
            SetCooldown(lightObject);
        }

        flickeringLights.Remove(lightObject); // Remove the light from the flickering list
        yield return new WaitForSeconds(0.1f); // Add a cooldown between flickering lights
        flickerCooldown = false; // Reset the global flicker cooldown
    }

    void SetCooldown(GameObject lightObject)
    {
        if (!flickerCooldowns.ContainsKey(lightObject))
        {
            flickerCooldowns.Add(lightObject, Time.time);
        }
        else
        {
            flickerCooldowns[lightObject] = Time.time;
        }
    }

    bool IsOnCooldown(GameObject lightObject)
    {
        if (flickerCooldowns.ContainsKey(lightObject))
        {
            float cooldownTime = flickerCooldowns[lightObject];
            float currentTime = Time.time;
            return (currentTime - cooldownTime) < 10f; // 10-second cooldown
        }
        return false;
    }

    void PlayRandomFlickerSound()
    {
        if (flickerAudioSource != null && flickerSounds.Length > 0)
        {
            int randomIndex = Random.Range(0, flickerSounds.Length); // Choose a random sound effect index
            flickerAudioSource.clip = flickerSounds[randomIndex]; // Set the audio source's clip to the chosen sound effect
            flickerAudioSource.PlayOneShot(flickerAudioSource.clip); // Play the flicker sound effect once
        }
    }

    void PlayRandomMonsterSound()
    {
        // Check if enough time has passed since the last monster sound
        if (Time.time - lastMonsterSoundTime >= monsterSoundCooldown)
        {
            if (flickerAudioSource != null && monsterSounds.Length > 0)
            {
                int randomIndex = Random.Range(0, monsterSounds.Length); // Choose a random sound effect index
                flickerAudioSource.clip = monsterSounds[randomIndex]; // Set the audio source's clip to the chosen sound effect
                flickerAudioSource.PlayOneShot(flickerAudioSource.clip); // Play the monster sound effect once

                lastMonsterSoundTime = Time.time; // Update the timestamp of the last monster sound played
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, lightDetectionRadius);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, turnOffLightsRadius);
    }
}
