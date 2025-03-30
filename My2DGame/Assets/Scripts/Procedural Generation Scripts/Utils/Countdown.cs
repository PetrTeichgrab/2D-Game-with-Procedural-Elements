using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Countdown : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI countdownText;

    [SerializeField]
    public float remainingTime = 25;

    public bool StartCountdown { get; set; }

    public bool CountdownFinished { get; set; }

    public bool isCountdownForFinalLevel { get; set; }

    public bool isCountdownForUnderground { get; set; }

    private AudioManager audioManager;

    private bool played20sSound = false;

    private void Start()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
        played20sSound = false;
    }

    void Update()
    {
        if (StartCountdown)
        {
            this.gameObject.SetActive(true);
            if (remainingTime >= 1)
            {
                remainingTime -= Time.deltaTime;
                if (remainingTime <= 20f && !played20sSound)
                {
                    played20sSound = true;
                    audioManager.PlayTickingSound();
                }
            }
            else
            {
                remainingTime = 0;
                CountdownFinished = true;
                this.gameObject.SetActive(false);
                played20sSound = false;
            }
            int minutes = Mathf.FloorToInt(remainingTime / 60);
            int seconds = Mathf.FloorToInt(remainingTime % 60);
            countdownText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
    }

    public void ResetRemainingTime()
    {
        remainingTime = 25;
        audioManager.StopTickingSound();
        played20sSound = false;
    }

    public void HideCountdown()
    {
        countdownText.text = string.Empty;
    }
}
