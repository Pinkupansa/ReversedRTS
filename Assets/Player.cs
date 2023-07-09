using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IDamageable
{
    public static Player instance;
    [SerializeField] int maxHealth;
    int currentHealth;

    [SerializeField] AudioClip[] damageSounds;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage, bool isImmobilisation)
    {
        Debug.Log("Player took damage");
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Die();
        }
        SoundUtility.PlayRandomFromArrayOneShot(GetComponent<AudioSource>(), damageSounds);

    }

    void Die()
    {
        Destroy(gameObject);
    }


}
