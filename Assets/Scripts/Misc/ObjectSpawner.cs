using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    public enum CameraSide { Left, Right, Up, Down};
    public CameraSide side;
    public BoxCollider2D spawnBox;
    public GameObject CivilianPefab;
    public GameObject AsteroidPrefab;

    public float civilianSpawnTime = 5f;
    public float asteroidSpawnTime = 5f;
    public void Start()
    {
        InvokeRepeating("SpawnCivilians", 0f, civilianSpawnTime);
        InvokeRepeating("SpawnAsteroids", 0f, asteroidSpawnTime);
    }

    
    private void SpawnCivilians()
    {
        Bounds colliderBounds = spawnBox.bounds;
        Vector3 colliderCenter = colliderBounds.center;

        GameObject spawnedCivilian = null;
        if (side.Equals(CameraSide.Up) || side.Equals(CameraSide.Down))
        {
            spawnedCivilian = Instantiate(CivilianPefab, new Vector2(DetermineSpawnPointX(colliderBounds, colliderCenter), colliderCenter.y), Quaternion.identity);
            spawnedCivilian.GetComponent<CivilianShipController>().MoveToPosition(new Vector2(DetermineSpawnPointX(colliderBounds, colliderCenter), -colliderCenter.y));
        }
        else if (side.Equals(CameraSide.Right) || side.Equals(CameraSide.Left))
        {
            spawnedCivilian = Instantiate(CivilianPefab, new Vector2(colliderCenter.x, DetermineSpawnPointY(colliderBounds, colliderCenter)), Quaternion.identity);
            spawnedCivilian.GetComponent<CivilianShipController>().MoveToPosition(new Vector2(-colliderCenter.x, DetermineSpawnPointY(colliderBounds, colliderCenter)));
        }
    }

    private void SpawnAsteroids()
    {
        Bounds colliderBounds = spawnBox.bounds;
        Vector3 colliderCenter = colliderBounds.center;

        GameObject spawnedAsteroid = null;
        if (side.Equals(CameraSide.Up) || side.Equals(CameraSide.Down))
        {
            Instantiate(AsteroidPrefab, new Vector2(DetermineSpawnPointX(colliderBounds, colliderCenter), colliderCenter.y), Quaternion.identity);
            spawnedAsteroid.GetComponent<Asteroid>().destination = new Vector2(DetermineSpawnPointX(colliderBounds, colliderCenter), -colliderCenter.y);
        }
        else if (side.Equals(CameraSide.Right) || side.Equals(CameraSide.Left))
        {
            Instantiate(AsteroidPrefab, new Vector2(colliderCenter.x, DetermineSpawnPointY(colliderBounds, colliderCenter)), Quaternion.identity);
            spawnedAsteroid.GetComponent<Asteroid>().destination = new Vector2(-colliderCenter.x, DetermineSpawnPointY(colliderBounds, colliderCenter));
        }
    }

    public float DetermineSpawnPointX(Bounds colliderBounds, Vector3 colliderCenter)
    {
        float randomX = Random.Range(colliderCenter.x - colliderBounds.extents.x, colliderCenter.x + colliderBounds.extents.x);

        return randomX;
    }

    public float DetermineSpawnPointY(Bounds colliderBounds, Vector3 colliderCenter)
    {
        float randomY = Random.Range(colliderCenter.y - colliderBounds.extents.y, colliderCenter.y + colliderBounds.extents.y);

        return randomY;
    }
}
