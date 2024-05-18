using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawning : MonoBehaviour
{
    public GameObject shadowMonsterPrefab;
    public GameObject night;
    public float spawnRate = 5f; // The rate at which monsters are spawned (in seconds)
    private List<GameObject> spawnedMonsters = new List<GameObject>();
    private Coroutine spawnCoroutine;
    public float spawnRadius = 5.0f;
    private GameObject player;

    void Start(){
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        if (night.activeInHierarchy && spawnCoroutine == null)
        {
            // If the night GameObject is active and the spawn coroutine is not running, start the spawn coroutine
            spawnCoroutine = StartCoroutine(SpawnMonsters());
        }
        else if (!night.activeInHierarchy && spawnCoroutine != null)
        {
            // If the night GameObject is not active and the spawn coroutine is running, stop the spawn coroutine and destroy all spawned monsters
            StopCoroutine(spawnCoroutine);
            spawnCoroutine = null;
            foreach (GameObject monster in spawnedMonsters)
            {
                Destroy(monster);
            }
            spawnedMonsters.Clear();
        }
    }

    IEnumerator SpawnMonsters()
    {
        while (true)
        {
            // Spawn a monster at the position of the nest GameObject
            Vector3 spawnPosition = player.transform.position + Random.insideUnitSphere * spawnRadius;
            spawnPosition.y = transform.position.y;
            GameObject monster = Instantiate(shadowMonsterPrefab, spawnPosition, Quaternion.identity);
            spawnedMonsters.Add(monster);
            yield return new WaitForSeconds(spawnRate);
        }
    }
}