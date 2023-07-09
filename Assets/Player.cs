using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IDamageable
{
    public static Player instance;
    [SerializeField] int maxHealth;
    [SerializeField] DiscreteLifeBar lifeBar;
    [SerializeField] GameObject graphics;
    int currentHealth;

    [SerializeField] AudioClip[] damageSounds;

    Coroutine blinkingCoroutine;
    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        currentHealth = maxHealth;
        lifeBar.Initialize(maxHealth);
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
        lifeBar.UpdateLife(currentHealth);
        if (blinkingCoroutine != null)
            StopCoroutine(blinkingCoroutine);
        blinkingCoroutine = StartCoroutine(Blinking());
        CamManager.instance.CamShake(1, 6);

    }

    void Die()
    {
        Destroy(gameObject);
    }

    IEnumerator Blinking()
    {
        for (int i = 0; i < 5; i++)
        {
            graphics.SetActive(false);
            yield return new WaitForSeconds(0.2f);
            graphics.SetActive(true);
            yield return new WaitForSeconds(0.2f);
        }
    }


}
