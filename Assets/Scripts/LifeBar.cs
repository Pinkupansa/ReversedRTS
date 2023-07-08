using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeBar : MonoBehaviour
{
    [SerializeField] GameObject lifeFill;
    [SerializeField] GameObject lifeBar;

    public void UpdateLifeBar(float currentLife, float maxLife)
    {
        float lifeRatio = currentLife / maxLife;
        lifeFill.transform.localScale = new Vector3(lifeRatio, 1, 1);
    }
}
