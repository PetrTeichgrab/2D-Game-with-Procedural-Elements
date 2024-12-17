using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StoryMenu : MonoBehaviour
{
    public void StartAdvanture()
    {
        SceneManager.LoadScene("Game");
    }
}
