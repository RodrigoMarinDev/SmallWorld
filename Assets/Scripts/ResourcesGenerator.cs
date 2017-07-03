using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourcesGenerator : MonoBehaviour
{
    [Header("Resource")]
    public GameObject resourcePrefab;
    public int minResourceSpawnTime;
    public int maxResourceSpawnTime;
    float lastResourceSpawnTime;

    [Header("Enemy")]
    public int firstTimeSpawn;
    public GameObject enemyPrefab;
    public float minEnemySpawnTime;
    public float maxEnemySpawnTime;
    public float reduceEnemySpawnTime;
    float lastEnemySpawnTime;

    BoxCollider2D Bounds;
    Vector3 maxBounds;
    Vector3 minBounds;

    void Awake()
    {
        Bounds = GetComponentInChildren<BoxCollider2D>();
        minBounds = Bounds.bounds.min;
        maxBounds = Bounds.bounds.max;
        lastEnemySpawnTime = Random.Range(firstTimeSpawn, firstTimeSpawn + 30) + Time.time;
        lastResourceSpawnTime = Random.Range(minResourceSpawnTime, maxResourceSpawnTime) + Time.time;
    }

    void Update()
    {
        if (Time.time > lastResourceSpawnTime)
        {
            lastResourceSpawnTime = Time.time + Random.Range(minResourceSpawnTime, maxResourceSpawnTime);
            Vector3 newResource = new Vector3(Random.Range(minBounds.x, maxBounds.x), Random.Range(minBounds.y, maxBounds.y), 0);
            Instantiate(resourcePrefab, newResource, Quaternion.identity, transform);
        }

        if (Time.time > lastEnemySpawnTime)
        {
            lastEnemySpawnTime = Time.time + Random.Range(minEnemySpawnTime, maxEnemySpawnTime);
            if (minEnemySpawnTime > 3)
                minEnemySpawnTime -= reduceEnemySpawnTime;
            if (maxEnemySpawnTime > 3)
                maxEnemySpawnTime -= (reduceEnemySpawnTime * 4);
            Vector3 newEnemy = new Vector3(Random.Range(minBounds.x, maxBounds.x), Random.Range(minBounds.y, maxBounds.y), 0);
            Instantiate(enemyPrefab, newEnemy, Quaternion.identity, transform);
        }
    }
}
