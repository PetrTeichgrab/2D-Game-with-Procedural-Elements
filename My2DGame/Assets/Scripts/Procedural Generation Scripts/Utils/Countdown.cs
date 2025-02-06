using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Countdown : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI countdownText;
    float elapsedTime = 60;
    void Start()
    {
        
    }

    void Update()
    {
        elapsedTime -= Time.deltaTime;
        int minutes = Mathf.FloorToInt(elapsedTime / 60);
        int seconds = Mathf.FloorToInt(elapsedTime % 60);
        countdownText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
