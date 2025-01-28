using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UndergroundBehaviour : DungeonBehaviour
{
    [SerializeField]
    Item saveItem;

    [SerializeField]
    Player Player;

    [SerializeField]
    DungeonGenerator generator;

    private bool isPlayerInUnderground;

    private void Update()
    {
        if (!isPlayerInUnderground)
        {
            PlacePlayerAtHighestPosition(Player);
            Player.SetTransparency(0.1f);
            Player.EnableGravityMode();
            isPlayerInUnderground = true;
        }
    }
    private void PlacePlayerAtHighestPosition(Player player)
    {
        Vector2Int highestPosition = new Vector2Int(int.MinValue, int.MinValue);
        foreach (var position in generator.undergroundDungeon.Floor)
        {
            if (position.y > highestPosition.y || (position.y == highestPosition.y && position.x > highestPosition.x))
            {
                highestPosition = position;
            }
        }

        if (player != null)
        {
            player.transform.position = new UnityEngine.Vector3(highestPosition.x, highestPosition.y + 2, player.transform.position.z);
            Debug.Log("Player placed in position: " + new UnityEngine.Vector3(highestPosition.x, highestPosition.y, player.transform.position.z));
        }
        else
        {
            Debug.LogWarning("Player reference is missing.");
        }
    }
}
