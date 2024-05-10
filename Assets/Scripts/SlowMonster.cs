using UnityEngine;
using UnityEngine.AI;

public class ChasingMonster : MonoBehaviour
{
    private GameObject player; // Reference to the player GameObject
    private NavMeshAgent navMeshAgent;
    public float damageRadius = 1.5f;
    public float initialSpeed = 2f; // Initial slow speed
    public float speedIncreaseRate = 0.5f; // Rate of speed increase per second (adjusted for faster increase)
    public float speedIncreaseInterval = 2f; // Interval for speed increase
    private float timer = 0f; // Timer for speed increase
    public float damageAmount = 10f; // Amount of damage inflicted on the player
    public float damageCooldown = 2f; // Cooldown time between damage ticks
    private float lastDamageTime = 0f; // Timestamp of the last damage
    public float currentSpeed; // Current speed for debugging

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.speed = initialSpeed; // Set initial speed
        currentSpeed = initialSpeed; // Set current speed for debugging
    }

    void Update()
    {
        if (player != null)
        {
            navMeshAgent.SetDestination(player.transform.position);

            // Check if it's time to increase speed
            timer += Time.deltaTime;
            if (timer >= speedIncreaseInterval)
            {
                timer = 0f;
                IncreaseSpeed();
            }

            // Check if the player is nearby using an overlap sphere
            Collider[] colliders = Physics.OverlapSphere(transform.position, damageRadius);
            foreach (Collider collider in colliders)
            {
                if (collider.CompareTag("Player"))
                {
                    DamagePlayer();
                }
            }
        }
    }

    void IncreaseSpeed()
    {
        // Increase speed exponentially
        currentSpeed *= Mathf.Pow(1 + speedIncreaseRate, Time.deltaTime);
        navMeshAgent.speed = currentSpeed;
    }

    void DamagePlayer()
    {
        if (Time.time - lastDamageTime >= damageCooldown)
        {
            Player playerScript = player.GetComponent<Player>();
            if (playerScript != null)
            {
                playerScript.setHealth(playerScript.currentHealth - damageAmount);
                lastDamageTime = Time.time;
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, damageRadius);
    }
}
