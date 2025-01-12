using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class noiseMovement : MonoBehaviour
{
    public float movementRadius = 1f;
    public float movementSpeed = 0.5f;
    public float noiseFrequency = 1f;

    private Vector3 originalPosition;

    private void Start()
    {
        originalPosition = transform.position;

        StartCoroutine(MoveLight());
    }

    private IEnumerator MoveLight()
    {
        while (true)
        {
            float offsetX = Mathf.PerlinNoise(Time.time * noiseFrequency, 0) * 2 - 1;
            float offsetY = Mathf.PerlinNoise(0, Time.time * noiseFrequency) * 2 - 1;

            Vector3 targetPosition = originalPosition + new Vector3(offsetX, offsetY, 0) * movementRadius;

            while (Vector3.Distance(transform.position, targetPosition) > 0.1f)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, movementSpeed * Time.deltaTime);
                yield return null;
            }

            yield return new WaitForSeconds(Random.Range(0f, 0.1f));
        }
    }
}
