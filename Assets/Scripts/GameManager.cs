using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    int numberOfAliveCaserns;

    public void Awake()
    {
        instance = this;

    }
    void Start()
    {
        numberOfAliveCaserns = FindObjectsOfType<Caserne>().Length;
    }

    public void OnCasernDestroyed()
    {
        numberOfAliveCaserns--;
        if (numberOfAliveCaserns <= 0)
        {
            Debug.Log("Game Over");
        }

        //call all caserns to update their probabilities
        Caserne[] caserns = FindObjectsOfType<Caserne>();
        foreach (Caserne casern in caserns)
        {
            casern.OnCasernDestroyed();
        }
    }

    public int GetNumberOfAliveCaserns()
    {
        return numberOfAliveCaserns;
    }
}
