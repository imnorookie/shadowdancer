using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathManager : MonoBehaviour
{
    public GameObject youDiedText;
    public GameObject youreInjuredText;
    public GameObject InjuredScale;

    private bool isInjured = false;

    private bool gamePaused = false;

    private void Update()
    {
        if (gamePaused && Input.GetKeyDown(KeyCode.R))
        {
            Time.timeScale = 1.0f; // Resume the game.
            youDiedText.SetActive(false);
            youreInjuredText.SetActive(false);
            InjuredScale.SetActive(false);
            string currentSceneName = SceneManager. GetActiveScene(). name;
            SceneManager.LoadScene(currentSceneName);
        }
    }

    public void handleAttackResult()
    {
        if (isInjured)
        {
            ShowDeathMessage();            
        }
        else
        {
            ShowDamagedScreen();
        }
    }

    public void ShowDeathMessage()
    {
        Time.timeScale = 0.0f; // Pause the game.
        youDiedText.SetActive(true);
        gamePaused = true;
    }
    public void ShowDamagedScreen()
    {
        youreInjuredText.SetActive(true);
        InjuredScale.SetActive(true);
        isInjured = true;
    }

    public void RemoveDamagedScreen()
    {
        Debug.Log("Function called");
        youreInjuredText.SetActive(false);
        InjuredScale.SetActive(false);
        isInjured = false;
    }   
}