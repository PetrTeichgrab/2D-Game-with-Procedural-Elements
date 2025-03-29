using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public Vector3 offset = new Vector3(0, 5, -10);
    public float smoothSpeed = 0.125f;

    private Vector3 originalOffset;
    private Coroutine zoomCoroutine;

    private void Start()
    {
        originalOffset = offset;
        target = Player.Instance.transform;
    }

    private void LateUpdate()
    {
        if (target == null) return;
        Vector3 desiredPosition = target.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;
    }

    public void ZoomOutTemporarily(Vector3 zoomedOutOffset, float duration, float transitionTime = 1f)
    {
        if (zoomCoroutine != null)
            StopCoroutine(zoomCoroutine);

        zoomCoroutine = StartCoroutine(ZoomCoroutine(zoomedOutOffset, duration, transitionTime));
    }

    private IEnumerator ZoomCoroutine(Vector3 zoomedOutOffset, float duration, float transitionTime)
    {
        Vector3 startOffset = offset;

        float t = 0;
        while (t < 1)
        {
            offset = Vector3.Lerp(startOffset, zoomedOutOffset, t);
            t += Time.deltaTime / transitionTime;
            yield return null;
        }
        offset = zoomedOutOffset;

        yield return new WaitForSeconds(duration);

        t = 0;
        while (t < 1)
        {
            offset = Vector3.Lerp(zoomedOutOffset, originalOffset, t);
            t += Time.deltaTime / transitionTime;
            yield return null;
        }
        offset = originalOffset;
    }
}

