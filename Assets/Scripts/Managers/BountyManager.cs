using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class BountyManager : MonoBehaviour
{
    public ScenesData sceneDB;
    public static BountyManager instance;
    
    [Header("General Stats")]
    public int bounty;
    public bool hasStarted;

    [Header("Enemy Spawn Stats")]
    public int maxEnemiesAtOnce;
    public int enemiesToSpawn = 0;

    [Header("Special Enemy Spawn Stats")]
    public int maxSpecialEnemiesAtOnce;
    public int specialEnemiesToSpawn = 0;

    [Header("Spawners")]
    public ObjectSpawner[] enemySpawners;
    public ObjectSpawner[] civilianSpawners;

    [Header("Spawned Objects")]
    private List<GameObject> spawnedEnemies = new List<GameObject>();
    private List<GameObject> spawnedCivilians = new List<GameObject>();


    public void Initialize()
    {
        instance = this;
    }

    public void ResetGame()
    {
        timeNeededToSpawn = UnityEngine.Random.Range(0.2f, 3.0f);
        hasStarted = false;
    }


    private void Update()
    {
        SpawnEnemies();
        SpawnCivilians();
        SpawnSpecialEnemies();
        UpdateBounty();
    }

    private void SpawnEnemies()
    {
        if (hasStarted)
        {
            HandleEnemySpawning();
        }
        else
        {
            AllocateEnemies();
        }
    }

    private float spawnTimer = 0;
    private float timeNeededToSpawn = 0;
    void HandleEnemySpawning()
    {
        spawnTimer += Time.deltaTime;
        maxEnemiesAtOnce = Mathf.Clamp((int)(bounty * 0.0005f), 1, 60);
        if (spawnTimer >= timeNeededToSpawn && enemiesToSpawn > 0 && spawnedEnemies.Count(x => x != null) < maxEnemiesAtOnce)
        {
            enemiesToSpawn--;
            spawnTimer = 0;
            ObjectSpawner spawnEdge = enemySpawners[UnityEngine.Random.Range(0, enemySpawners.Length)];

            spawnedEnemies.Add(spawnEdge.SpawnEnemy(sceneDB.currentLevel.PickRandomEnemy()));
            timeNeededToSpawn = UnityEngine.Random.Range(1f, 7.0f);
        }
        else if (enemiesToSpawn == 0)
        {
            Debug.Log("Reallocating!");
            AllocateEnemies();
            spawnTimer = 0;
            timeNeededToSpawn = UnityEngine.Random.Range(6f, 14f);
        }
    }

    private void AllocateEnemies()
    {
        enemiesToSpawn = 5 + (int)(bounty * 0.00005f);
        hasStarted = true;
    }

    float civSpawnTimer = 0;
    float timeNeededToSpawnCiv = 0;
    private void SpawnCivilians()
    {
        if (!hasStarted)
            return;

        civSpawnTimer += Time.deltaTime;
        if (civSpawnTimer >= timeNeededToSpawnCiv)
        {
            civSpawnTimer = 0;
            ObjectSpawner spawnEdge = civilianSpawners[UnityEngine.Random.Range(0, civilianSpawners.Length)];

            spawnedCivilians.Add(spawnEdge.SpawnCivilian(sceneDB.currentLevel.PickRandomCivilian()));
            timeNeededToSpawnCiv = sceneDB.currentLevel.CalculateCivilianSpawnTime();
        }
    }

    private void SpawnSpecialEnemies()
    {
        if (hasStarted)
        {
            HandleSpecialEnemySpawning();
        }
        else
        {
            AllocateSpecialEnemies();
        }
    }

    private float specialEnemySpawnTimer = 0;
    private float timeNeededToSpawnSpecialEnemy = 0;
    private void HandleSpecialEnemySpawning()
    {
        spawnTimer += Time.deltaTime;
        maxSpecialEnemiesAtOnce = Mathf.Clamp((int)(bounty * 0.00005f), 1, 20);
        if (spawnTimer >= timeNeededToSpawn && enemiesToSpawn > 0 && spawnedEnemies.Count(x => x != null) < maxEnemiesAtOnce)
        {
            enemiesToSpawn--;
            spawnTimer = 0;
            ObjectSpawner spawnEdge = enemySpawners[UnityEngine.Random.Range(0, enemySpawners.Length)];

            spawnedEnemies.Add(spawnEdge.SpawnEnemy(sceneDB.currentLevel.PickRandomEnemy()));
            timeNeededToSpawn = UnityEngine.Random.Range(120f, 240f);
        }
        else if (enemiesToSpawn == 0)
        {
            Debug.Log("Reallocating!");
            AllocateSpecialEnemies();
            spawnTimer = 0;
        }
    }

    private void AllocateSpecialEnemies()
    {
        enemiesToSpawn = 1 + (int)(bounty * 0.000005f);
        hasStarted = true;
    }

    private float gameTime;
    private void UpdateBounty()
    {
        gameTime += Time.deltaTime;
        if (gameTime >= 1)
        {
            AddBounty(100);
            gameTime = 0;
        }
    }

    public void AddBounty(int bountyPoints)
    {
        bounty += bountyPoints;
    }
}
