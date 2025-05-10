using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject mainMenuUI;
    [SerializeField] private GameObject continueMenuUI;
    [SerializeField] private GameObject optionsMenuUI;

    private void Start()
    {
        optionsMenuUI.SetActive(false);
        continueMenuUI.SetActive(false);
    }
    public void Play()
    {
        mainMenuUI.SetActive(false);
        continueMenuUI.SetActive(true);
    }

    public void Options()
    {
        mainMenuUI.SetActive(false);
        continueMenuUI.SetActive(false);
        optionsMenuUI.SetActive(true);
    }

    public void Back()
    {
        mainMenuUI.SetActive(true);
        continueMenuUI.SetActive(false);
        optionsMenuUI.SetActive(false);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
