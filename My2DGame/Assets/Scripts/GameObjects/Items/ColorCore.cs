using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using System.Collections;
using UnityEngine.Rendering.Universal;

public class ColorCore : Item
{
    [SerializeField]
    protected Animator animator;

    public bool isPlaced { get; set; }

    private Light2D light2D;

    private void Awake()
    {
        light2D = GetComponent<Light2D>();
        if (light2D == null)
        {
            Debug.LogWarning("Light2D component is missing on the ColorCore object.");
        }
    }
    public void GradualLightIncrease(float duration)
    {
        if (light2D == null) return;

        StartCoroutine(GradualLightIncreaseCoroutine(duration));
    }

    private IEnumerator GradualLightIncreaseCoroutine(float duration)
    {
        float originalIntensity = light2D.intensity;
        float originalOuterRadius = light2D.pointLightOuterRadius;

        float targetIntensity = originalIntensity*1.2f; // Tøikrát vyšší jas
        float targetOuterRadius = originalOuterRadius * 65; // Dvojnásobný dosah

        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            light2D.intensity = Mathf.Lerp(originalIntensity, targetIntensity, elapsedTime / duration);
            light2D.pointLightOuterRadius = Mathf.Lerp(originalOuterRadius, targetOuterRadius, elapsedTime / duration);

            elapsedTime += Time.deltaTime;

            yield return null;
        }

        // Nastavit cílové hodnoty, aby interpolace pøesnì skonèila na požadovaných èíslech
        light2D.intensity = targetIntensity;
        light2D.pointLightOuterRadius = targetOuterRadius;

        Debug.Log("Light increase completed.");
    }
}
