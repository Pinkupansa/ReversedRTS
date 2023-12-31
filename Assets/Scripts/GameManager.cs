using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    int numberOfAliveCaserns;
    [SerializeField] TMPro.TMP_Text timerText;
    [SerializeField] GameObject deadScreen, winScreen;
    float timer;
    bool gameOver;
    public void Awake()
    {
        instance = this;

    }
    void Start()
    {
        numberOfAliveCaserns = FindObjectsOfType<Caserne>().Length;
        timer = 0;
        deadScreen.SetActive(false);
        winScreen.SetActive(false);
    }

    public void OnCasernDestroyed()
    {
        numberOfAliveCaserns--;
        if (numberOfAliveCaserns <= 0)
        {
            GameObject.FindObjectOfType<Nexus>().Unshield();
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

    void UpdateTimerText()
    {
        //keep 2 decimals
        timerText.text = timer.ToString("F2");
    }

    void Update()
    {
        timer += Time.deltaTime;
        UpdateTimerText();
    }

    public void OnPlayerDeath()
    {
        gameOver = true;
        deadScreen.SetActive(true);
        timerText.gameObject.SetActive(false);
    }

    public void OnWin()
    {
        gameOver = true;
        winScreen.SetActive(true);
        winScreen.transform.GetChild(0).GetComponent<TMPro.TMP_Text>().text = "You won in " + timer.ToString("F2") + " seconds! Congratulations!";
        timerText.gameObject.SetActive(false);
    }

    public bool IsGameOver()
    {
        return gameOver;
    }

    public void Restart()
    {
        //load the CURRENT scene
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
    }

    public void MainMenu()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }
}
