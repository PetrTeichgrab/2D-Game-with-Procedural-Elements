using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using System.Collections;
using UnityEngine.Rendering.Universal;

public class ColorCore : Item
{
    [SerializeField]
    protected Animator animator;

    public DungeonColor color;

    public bool isPlaced { get; set; }

    private Light2D light2D;

    protected AudioManager audioManager;

    private void Awake()
    {
        light2D = GetComponent<Light2D>();
        if (light2D == null)
        {
            Debug.LogWarning("Light2D component is missing on the ColorCore object.");
        }
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    protected void playPickUpSFX()
    {
        audioManager.PlaySFX(audioManager.playerPickUpColorCore);
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

        float targetIntensity = originalIntensity*1.2f;
        float targetOuterRadius = originalOuterRadius * 65;

        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            light2D.intensity = Mathf.Lerp(originalIntensity, targetIntensity, elapsedTime / duration);
            light2D.pointLightOuterRadius = Mathf.Lerp(originalOuterRadius, targetOuterRadius, elapsedTime / duration);

            elapsedTime += Time.deltaTime;

            yield return null;
        }

        light2D.intensity = targetIntensity;
        light2D.pointLightOuterRadius = targetOuterRadius;

        Debug.Log("Light increase completed.");
    }
}
