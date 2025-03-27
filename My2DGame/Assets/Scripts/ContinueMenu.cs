using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ContinueMenu : MonoBehaviour
{
    public void NewGame()
    {
        SaveSystem.ResetStats();
        SceneManager.LoadScene("StoryScene");
    }

    public void Continue()
    {
        SceneManager.LoadScene("StoryScene");
    }
}
