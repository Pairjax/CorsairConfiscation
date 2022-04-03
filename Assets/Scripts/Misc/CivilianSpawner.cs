using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CivilianSpawner : MonoBehaviour
{
    public enum CameraSide { Left, Right, Up, Down};
    public CameraSide side;
    public BoxCollider2D spawnBox;
    public GameObject CivilianPefab;

    public float spawnTime = 5f;

    public void Start()
    {
        InvokeRepeating("SpawnCivilian", 0f, spawnTime);
    }

    private void SpawnCivilian()
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
