using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Enemy")]
    public GameObject enemyPrefab;

    [Header("Spawn Settings")]
    public int maxEnemies = 10;
    public float spawnInterval = 1.2f;
    public float spawnRadius = 8f;

    private float spawnTimer;
    private List<GameObject> aliveEnemies = new List<GameObject>();

    void Update()
    {
        spawnTimer += Time.deltaTime;

        if (spawnTimer >= spawnInterval && aliveEnemies.Count < maxEnemies)
        {
            SpawnEnemy();
            spawnTimer = 0f;
        }

        CleanList();
    }

    void SpawnEnemy()
    {
        Vector2 spawnPos = (Vector2)transform.position + Random.insideUnitCircle.normalized * spawnRadius;

        GameObject enemy = Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
        aliveEnemies.Add(enemy);
    }

    void CleanList()
    {
        aliveEnemies.RemoveAll(enemy => enemy == null);
    }
}
