using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StoryMenu : MonoBehaviour
{
    void Start()
    {
        Time.timeScale = 1;
    }
    void Awake()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void StartAdvanture()
    {
        SceneManager.LoadScene("Game");
    }
}
