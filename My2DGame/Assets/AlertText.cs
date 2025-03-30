using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AlertText : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI alertText;
    [SerializeField] private float displayDuration = 2f;

    public static AlertText Instance { get; private set; }

    private Coroutine activeCoroutine;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        if (alertText != null)
        {
            alertText.gameObject.SetActive(false);
        }
    }

    public void ShowAlert(string message, float duration = -1f)
    {
        if (alertText == null) return;

        if (activeCoroutine != null)
        {
            StopCoroutine(activeCoroutine);
        }

        alertText.text = message;
        alertText.gameObject.SetActive(true);

        activeCoroutine = StartCoroutine(HideAfter(duration > 0 ? duration : displayDuration));
    }

    private IEnumerator HideAfter(float time)
    {
        yield return new WaitForSeconds(time);
        alertText.gameObject.SetActive(false);
        activeCoroutine = null;
    }
}

