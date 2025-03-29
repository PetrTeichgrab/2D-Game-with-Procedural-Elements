using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Cinemachine.CinemachineVirtualCamera mainVirtualCamera;
    public float zoomOutSize = 40f;
    public float transitionTime = 2f;
    public float stayDuration = 5f;

    private float originalSize;
    private Coroutine zoomCoroutine;

    private void Awake()
    {
        if (mainVirtualCamera == null)
        {
            mainVirtualCamera = FindObjectOfType<CinemachineVirtualCamera>();
        }
    }

    public void ActivateZoomOut()
    {
        if (zoomCoroutine != null)
            StopCoroutine(zoomCoroutine);

        zoomCoroutine = StartCoroutine(ZoomOutCoroutine());
    }

    private IEnumerator ZoomOutCoroutine()
    {
        if (mainVirtualCamera == null)
        {
            Debug.LogWarning("Virtual Camera not assigned!");
            yield break;
        }

        var lens = mainVirtualCamera.m_Lens;

        originalSize = lens.Orthographic ? lens.OrthographicSize : lens.FieldOfView;
        float targetSize = zoomOutSize;

        yield return ZoomCamera(originalSize, targetSize, transitionTime);

        yield return new WaitForSeconds(stayDuration);

        yield return ZoomCamera(targetSize, originalSize, transitionTime);
    }

    private IEnumerator ZoomCamera(float from, float to, float duration)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            float current = Mathf.Lerp(from, to, t);

            if (mainVirtualCamera.m_Lens.Orthographic)
            {
                mainVirtualCamera.m_Lens.OrthographicSize = current;
            }
            else
            {
                mainVirtualCamera.m_Lens.FieldOfView = current;
            }

            Debug.Log("Zooming: " + current);

            yield return null;
        }

        if (mainVirtualCamera.m_Lens.Orthographic)
        {
            mainVirtualCamera.m_Lens.OrthographicSize = to;
        }
        else
        {
            mainVirtualCamera.m_Lens.FieldOfView = to;
        }
    }
}

