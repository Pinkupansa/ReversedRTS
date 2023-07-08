using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Caserne : MonoBehaviour, IDamageable
{
    [SerializeField] float spawnRate;
    [SerializeField] GameObject soldierPrefab;
    [SerializeField] Transform spawnPoint;

    [SerializeField] int maxHealth;

    [SerializeField] LifeBar lifeBar;
    int currentHealth;

    float timer;
    float nextSpawnTime;
    void Start()
    {
        nextSpawnTime = 1 / spawnRate + Random.Range(0, 1 / spawnRate);
        currentHealth = maxHealth;
        lifeBar.UpdateLifeBar(currentHealth, maxHealth);
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

    public void TakeDamage(int damage, bool isImmobilisation = false)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Die();
        }
        lifeBar.UpdateLifeBar(currentHealth, maxHealth);
    }

    private void Die()
    {
        Debug.Log("Caserne destroyed");
        Destroy(gameObject);
    }



}
