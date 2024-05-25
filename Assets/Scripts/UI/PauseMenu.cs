using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenu;

    public bool isPaused;

    // Start is called before the first frame update
    void Start()
    {
        if (pauseMenu == null)
        {
            pauseMenu = GameObject.Find("PauseMenu");
        }
        pauseMenu.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if(isPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Pause()
    {
        UnityEngine.Cursor.visible = true;
        UnityEngine.Cursor.lockState = CursorLockMode.None;
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;

        AudioSource[] audios = FindObjectsOfType<AudioSource>();

        foreach (AudioSource audio in audios)
        {
            audio.Pause();
        }
    }

    public void Resume()
    {
        UnityEngine.Cursor.visible = false;
        UnityEngine.Cursor.lockState = CursorLockMode.Locked;
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;

        AudioSource[] audios = FindObjectsOfType<AudioSource>();

        foreach (AudioSource audio in audios)
        {
            audio.UnPause();
        }
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void MainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Menu");
    }
}
