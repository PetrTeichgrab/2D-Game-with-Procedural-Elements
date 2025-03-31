using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class controlls : MonoBehaviour
{
    [SerializeField] GameObject Controlls;

    [SerializeField] private Button closeBtn;

    private void Start()
    {
        Controlls.gameObject.SetActive(false);
        closeBtn.onClick.AddListener(Close);
    }

    private void Close()
    {
        Controlls.gameObject.SetActive(false);  
    }

}
