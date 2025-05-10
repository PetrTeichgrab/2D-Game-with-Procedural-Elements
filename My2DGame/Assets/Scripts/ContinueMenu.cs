using UnityEngine;
using UnityEngine.SceneManagement;

public class ContinueMenu : MonoBehaviour
{
    [SerializeField] private GameObject mainMenuUI;
    [SerializeField] private GameObject continueMenuUI;
    [SerializeField] private GameObject continueButton;

    void Start()
    {
        bool hasPlayed = PlayerPrefs.GetInt("HasPlayed", 0) == 1;
        continueButton.SetActive(hasPlayed);
    }

    public void NewGame()
    {
        SaveSystem.ResetStats();
        PlayerPrefs.SetInt("HasPlayed", 1);
        PlayerPrefs.Save();
        SceneManager.LoadScene("ControllsScene");
    }

    public void Continue()
    {
        SceneManager.LoadScene("ControllsScene");
    }

    public void Back()
    {
        mainMenuUI.SetActive(true);
        continueMenuUI.SetActive(false);
    }
}
