using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelObject", menuName = "Scene Data/Level")]
public class Level : GameScene
{
    public enum LevelType { RestStation, Obstacle, Normal, Dead, Hive, Defense };
    public LevelType levelType;
    
    [Header("Enemies")]
    public GameObject[] enemyTypes;

    [Header("Special Enemies")]
    public GameObject[] specialEnemyTypes;

    [Header("Civilians")]
    public GameObject[] civilianTypes;
    [SerializeField] private float minSpawnTime;
    [SerializeField] private float maxSpawnTime;

    public GameObject PickRandomEnemy()
    {
        // Needs to accomodate spawn rates.
        return enemyTypes[UnityEngine.Random.Range(0, enemyTypes.Length)];
    }

    public GameObject PickRandomCivilian()
    {
        return civilianTypes[UnityEngine.Random.Range(0, civilianTypes.Length)];
    }

    public float CalculateCivilianSpawnTime()
    {
        return UnityEngine.Random.Range(minSpawnTime, maxSpawnTime);
    }

    public double GenerateRandomWeight()
    {
        return UnityEngine.Random.Range(0, 100);
    }
}
