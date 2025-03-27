using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject mainMenuUI;
    [SerializeField] private GameObject continueMenuUI;

    private void Start()
    {
        continueMenuUI.SetActive(false);
    }
    public void Play()
    {
        mainMenuUI.SetActive(false);
        continueMenuUI.SetActive(true);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
