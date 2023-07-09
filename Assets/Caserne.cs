using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Caserne : MonoBehaviour, IDamageable
{
    [SerializeField] float spawnRate;
    [SerializeField] GameObject[] soldierPrefabs;
    [SerializeField] Transform spawnPoint;
    float[] soldierProbabilities;
    [SerializeField] int maxHealth;
    [SerializeField] Sprite destroyedSprite;

    [SerializeField] LifeBar lifeBar;
    [SerializeField] GameObject particles;
    int currentHealth;

    float timer;
    float nextSpawnTime;

    bool alive = true;
    void Start()
    {
        nextSpawnTime = 1 / spawnRate + Random.Range(0, 1 / spawnRate);
        currentHealth = maxHealth;
        lifeBar.UpdateLifeBar(currentHealth, maxHealth);

        soldierProbabilities = new float[soldierPrefabs.Length];
        for (int i = 0; i < soldierPrefabs.Length; i++)
        {
            soldierProbabilities[i] = 0;
        }
        soldierProbabilities[0] = 1;

    }
    public void Update()
    {
        if (!alive)
        {
            return;
        }
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
        float random = Random.Range(0f, 1f);
        float sum = 0;
        int index = 0;
        while (sum < random)
        {
            sum += soldierProbabilities[index];
            index++;
        }
        index--;
        Debug.Log("Spawn soldier " + index);
        GameObject soldierPrefab = soldierPrefabs[index];
        GameObject soldier = Instantiate(soldierPrefab, spawnPoint.position, Quaternion.identity);

    }

    public void TakeDamage(int damage, bool isImmobilisation = false)
    {
        if (alive)
        {
            currentHealth -= damage;
            currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
            if (currentHealth <= 0)
            {
                Die();
                Instantiate(particles, transform.position, Quaternion.identity);
            }
            lifeBar.UpdateLifeBar(currentHealth, maxHealth);
        }

    }

    private void Die()
    {
        alive = false;
        Debug.Log("Caserne destroyed");
        GameManager.instance.OnCasernDestroyed();
        GetComponentInChildren<SpriteRenderer>().sprite = destroyedSprite;
        CamManager.instance.CamShake(2f, 8);
        //deactivate lifebar
        lifeBar.gameObject.SetActive(false);
    }

    public void OnCasernDestroyed()
    {
        Debug.Log("Update casern probabilities");
        switch (GameManager.instance.GetNumberOfAliveCaserns())
        {

            case 3:
                soldierProbabilities = new float[3];
                soldierProbabilities[0] = 0.5f;
                soldierProbabilities[1] = 0.5f;
                soldierProbabilities[2] = 0;
                spawnRate *= 1.33f;
                maxHealth = Mathf.RoundToInt(maxHealth * 1.2f);
                currentHealth = Mathf.RoundToInt(currentHealth * 1.2f);
                transform.localScale *= 1.2f;
                break;
            case 2:
                soldierProbabilities = new float[3];
                soldierProbabilities[0] = 0.5f;
                soldierProbabilities[1] = 0.25f;
                soldierProbabilities[2] = 0.25f;
                spawnRate *= 1.515f;
                maxHealth = Mathf.RoundToInt(maxHealth * 1.2f);
                currentHealth = Mathf.RoundToInt(currentHealth * 1.2f);
                transform.localScale *= 1.2f;
                break;
            case 1:
                soldierProbabilities = new float[3];
                soldierProbabilities[0] = 0.33f;
                soldierProbabilities[1] = 0.33f;
                soldierProbabilities[2] = 0.33f;
                spawnRate *= 2f;
                maxHealth = Mathf.RoundToInt(maxHealth * 1.2f);
                currentHealth = Mathf.RoundToInt(currentHealth * 1.2f);
                transform.localScale *= 1.2f;
                break;

        }

    }



}
