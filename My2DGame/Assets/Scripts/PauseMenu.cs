using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenu;

    public static bool isPaused;

    public GameObject shop;

    public GameObject controlls;

    private void Start()
    {
        pauseMenu.SetActive(false);
        shop.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }

        if (Input.GetKeyUp(KeyCode.H))
        {
            if (isPaused)
            {
                ResumeGame();
                shop.SetActive(false);
            }
            else
            {
                PauseGameWithoutShowingMenu();
                shop.SetActive(true);
            }
        }
    }

    public void PauseGame()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0;
        isPaused = true;
    }

    public void ShowControlls()
    {
        controlls.gameObject.SetActive(true);
    }

    public void PauseGameWithoutShowingMenu()
    {
        Time.timeScale = 0;
        isPaused = true;
    }

    public void ResumeGame()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Menu");
    }

    public void Quit() { 
        Application.Quit();
    }
}
