using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Caserne : MonoBehaviour
{
    [SerializeField] float spawnRate;
    [SerializeField] GameObject soldierPrefab;
    [SerializeField] Transform spawnPoint;

    float timer;
    float nextSpawnTime;
    void Start()
    {
        nextSpawnTime = 1 / spawnRate + Random.Range(0, 1 / spawnRate);
    }
    public void Update()
    {
        timer += Time.deltaTime;
        if (timer >= nextSpawnTime)
        {
            timer = 0;
            nextSpawnTime = 1 / spawnRate + Random.Range(0, 1 / spawnRate);
            SpawnSoldier();
        }
    }

    void SpawnSoldier()
    {
        GameObject soldier = Instantiate(soldierPrefab, spawnPoint.position, Quaternion.identity);

    }

}
