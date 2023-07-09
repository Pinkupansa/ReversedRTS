using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiscreteLifeBar : MonoBehaviour
{
    [SerializeField] GameObject heartPrefab;

    GameObject[] hearts;
    int maxHealth;
    int currentHealth;

    public void Initialize(int maxHealth)
    {
        this.maxHealth = maxHealth;
        currentHealth = maxHealth;
        hearts = new GameObject[maxHealth];
        for (int i = 0; i < maxHealth; i++)
        {
            GameObject heart = Instantiate(heartPrefab, transform);
            //put hearts so that the lifebar is centered
            heart.transform.localPosition = new Vector3((i - maxHealth / 2) * 100, 0, 0);


            hearts[i] = heart;

        }
    }
    public void UpdateLife(int currentHealth)
    {
        for (int i = 0; i < maxHealth; i++)
        {
            if (i < currentHealth)
            {
                hearts[i].transform.GetChild(0).gameObject.SetActive(true);
            }
            else
            {
                hearts[i].transform.GetChild(0).gameObject.SetActive(false);
            }
        }
    }
}
