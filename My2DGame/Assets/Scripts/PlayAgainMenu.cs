using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayAgainMenu : MonoBehaviour
{
    [SerializeField] private GameObject loadingScreen;
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private Slider loadingSlider;

    public void LoadGame()
    {
        loadingScreen.SetActive(true);
        StartCoroutine(LoadGameAsync());
        //mainMenu.SetActive(false);
    }
    IEnumerator LoadGameAsync()
    {
        AsyncOperation loadOp = SceneManager.LoadSceneAsync("game");
        Debug.Log("Started loading");

        while (!loadOp.isDone)
        {
            float progressVal = Mathf.Clamp01(loadOp.progress / 0.9f);
            loadingSlider.value = progressVal;
            yield return null;
        }
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Menu");
    }
}
