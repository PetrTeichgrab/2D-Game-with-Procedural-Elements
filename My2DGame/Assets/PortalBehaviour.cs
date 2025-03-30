using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalBehaviour : MonoBehaviour
{
    public DungeonBehaviour TargetDungeon;
    public Transform TargetPosition;

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("JOOOOOO teleport");
        if (other.CompareTag("Player"))
        {
            if (TargetDungeon != null && TargetPosition != null)
            {
                other.transform.position = TargetPosition.position;
                Debug.Log("Teleportováno do " + TargetDungeon.name);
            }
        }
    }
}
