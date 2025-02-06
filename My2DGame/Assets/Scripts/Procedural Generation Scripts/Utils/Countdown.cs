using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Countdown : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI countdownText;

    [SerializeField]
    float remainingTime = 60;

    public bool StartCountdown { get; set; }

    public bool CountdownFinished { get; set; }

    void Update()
    {
        if (StartCountdown)
        {
            if (remainingTime >= 1)
            {
                remainingTime -= Time.deltaTime;
            }
            else
            {
                remainingTime = 0;
                CountdownFinished = true;
            }
            int minutes = Mathf.FloorToInt(remainingTime / 60);
            int seconds = Mathf.FloorToInt(remainingTime % 60);
            countdownText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
    }
}
