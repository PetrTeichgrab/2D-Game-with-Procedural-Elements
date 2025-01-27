using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UndergroundBehaviour : DungeonBehaviour
{
    [SerializeField]
    Item saveItem;

    [SerializeField]
    Player player;

    public Underground underground;

    private void Update()
    {

    }

    private void PlacePlayerAtHighestPosition()
    {
        if (underground == null || underground.Floor == null || underground.Floor.Count == 0)
        {
            Debug.LogWarning("Underground or Floor data is missing.");
            return;
        }

        Vector2Int highestPosition = new Vector2Int(int.MinValue, int.MinValue);
        foreach (var position in underground.Floor)
        {
            if (position.y > highestPosition.y || (position.y == highestPosition.y && position.x > highestPosition.x))
            {
                highestPosition = position;
            }
        }

        if (player != null)
        {
            player.transform.position = new Vector3(highestPosition.x, highestPosition.y, player.transform.position.z);
        }
        else
        {
            Debug.LogWarning("Player reference is missing.");
        }
    }
}
}
