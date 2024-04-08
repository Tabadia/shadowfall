using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.AI;

public class ShadowMonster : MonoBehaviour
{
    public float largeRadius = 10f;
    public float smallRadius = 4f;
    public int maxFlickerCount = 3;
    public float clearTurnedOffInterval = 60f;
    public float clearFlickeredInterval = 30f;
    public bool canGoThruWalls = false;
    public GameObject player;

    private List<GameObject> flickeredLights = new List<GameObject>();
    private List<GameObject> turnedOffLights = new List<GameObject>();

    public NavMeshAgent agent;

    void Start()
    {
        InvokeRepeating("ClearTurnedOffLights", clearTurnedOffInterval, clearTurnedOffInterval);
        InvokeRepeating("ClearFlickeredLights", clearFlickeredInterval, clearFlickeredInterval);
    }

    void Update()
    {
        CheckLights();

        agent.destination = player.transform.position;

        // if (canGoThruWalls)
        // {
        //     agent.obstacleAvoidanceType = ObstacleAvoidanceType.NoObstacleAvoidance; // Disable obstacle avoidance
        //     print("Can go thru walls");
        // }
        // else
        // {
        //     agent.obstacleAvoidanceType = ObstacleAvoidanceType.HighQualityObstacleAvoidance; // Enable obstacle avoidance
        // }
    }

    void CheckLights()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, largeRadius);
        foreach (Collider col in colliders)
        {
            if (col.CompareTag("Light"))
            {
                GameObject lightObject = col.gameObject;
                if (!flickeredLights.Contains(lightObject) && lightObject.transform.GetChild(0).gameObject.activeSelf &&!IsInSmallRadius(lightObject))
                {
                    StartCoroutine(FlickerLight(lightObject));
                    flickeredLights.Add(lightObject);
                }
            }
        }

        colliders = Physics.OverlapSphere(transform.position, smallRadius);
        bool allLightsOff = true;
        foreach (Collider col in colliders)
        {
            if (col.CompareTag("Light"))
            {
                GameObject lightObject = col.gameObject;
                if (!turnedOffLights.Contains(lightObject))
                {
                    TurnOffLight(lightObject);
                    turnedOffLights.Add(lightObject);
                }
                if(lightObject.transform.GetChild(0).gameObject.activeSelf)
                {
                    allLightsOff = false;
                }
            }
        }

        canGoThruWalls = allLightsOff;
    }

    IEnumerator FlickerLight(GameObject lightObject)
    {
        int flickerCount = 0;
        while (flickerCount < maxFlickerCount)
        {
            lightObject.transform.GetChild(0).gameObject.SetActive(false);
            yield return new WaitForSeconds(Random.Range(0.1f, 0.5f));
            lightObject.transform.GetChild(0).gameObject.SetActive(true);
            yield return new WaitForSeconds(Random.Range(0.1f, 0.5f));
            flickerCount++;
        }
        lightObject.transform.GetChild(0).gameObject.SetActive(true);
    }

    void TurnOffLight(GameObject lightObject)
    {
        lightObject.transform.GetChild(0).gameObject.SetActive(false);
    }

    void ClearTurnedOffLights()
    {
        turnedOffLights.Clear();
    }

    void ClearFlickeredLights()
    {
        flickeredLights.Clear();
    }

    bool IsInSmallRadius(GameObject lightObject)
    {
        float distance = Vector3.Distance(transform.position, lightObject.transform.position);
        return distance <= smallRadius;
    }
}
