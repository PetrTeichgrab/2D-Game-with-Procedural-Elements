using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Coins : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI countdownText;

    private void Update()
    {
        countdownText.text = Player.Instance.money.ToString();
    }
}
