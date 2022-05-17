using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class BountyManager : MonoBehaviour
{
    public static BountyManager instance;
    private List<GameObject> spawnedEnemies = new List<GameObject>();
    public GameObject enemyPrefab;

    public int bounty;
    public bool hasStarted;

    [System.Serializable]
    public struct ObjectSpawners
    {
        public ObjectSpawner down;
        public ObjectSpawner up;
        public ObjectSpawner left;
        public ObjectSpawner right;
    }
    public ObjectSpawner[] spawnEdges;
    public int enemiesToSpawn = 0;

    public void Initialize()
    {
        instance = this;
    }

    public void ResetGame()
    {
        timeNeededToSpawn = UnityEngine.Random.Range(0.2f, 3.0f);
        hasStarted = false;
    }

    private float spawnTimer = 0;
    private float timeNeededToSpawn = 0;
    void HandleSpawning()
    {
        spawnTimer += Time.deltaTime;
        int maxEnemiesAtOnce = Mathf.Clamp(bounty * 3, 6, 55);
        if (spawnTimer >= timeNeededToSpawn && enemiesToSpawn > 0 && spawnedEnemies.Count(x => x != null) < maxEnemiesAtOnce)
        {
            enemiesToSpawn--;
            spawnTimer = 0;
            ObjectSpawner spawnEdge = spawnEdges[UnityEngine.Random.Range(0, spawnEdges.Length)];
            
            spawnedEnemies.Add(spawnEdge.SpawnEnemy(enemyPrefab));
            timeNeededToSpawn = UnityEngine.Random.Range(1f, 7.0f);
        }
    }

    private void Update()
    {
            if (hasStarted)
            {
                HandleSpawning();
            }
            else
            {
                AllocateEnemies();
            }
    }

    private void AllocateEnemies()
    {
        enemiesToSpawn = 5 + (3 * bounty);
        hasStarted = true;
    }
}
