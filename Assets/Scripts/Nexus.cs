using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nexus : MonoBehaviour
{
    [SerializeField] int maxHealth;
    int currentHealth;
    [SerializeField] LifeBar lifeBar;
    bool isShielded;
    void Start()
    {
        currentHealth = maxHealth;
        lifeBar.UpdateLifeBar(currentHealth, maxHealth);
        lifeBar.gameObject.SetActive(false);
        isShielded = true;
    }
    public void TakeDamage(int damage)
    {
        if (isShielded)
        {
            return;
        }
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        if (currentHealth <= 0)
        {
            Die();
        }
    }
    private void Die()
    {
        lifeBar.gameObject.SetActive(false);
        GameManager.instance.OnWin();
        CamManager.instance.CamShake(5f, 8);
    }


    public void Unshield()
    {
        isShielded = false;
    }

}
