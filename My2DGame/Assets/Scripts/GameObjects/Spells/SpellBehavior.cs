using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellBehavior : MonoBehaviour
{
    public GameObject hitEffect;
    private void OnDestroy()
    {
        if (hitEffect != null)
        {
            var effect = Instantiate(hitEffect, transform.position, Quaternion.identity);
            if (effect != null)
            {
                Destroy(effect, 0.2f);
            }
        }
    }
}
