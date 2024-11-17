using System.Collections.Generic;
using UnityEngine;

public class Room
{
    public HashSet<Vector2Int> FloorList { get; set; } = new HashSet<Vector2Int>();
    public HashSet<Vector2Int> WallList { get; set; } = new HashSet<Vector2Int>();
    public Room(HashSet<Vector2Int> floorList)
    {
        FloorList = floorList;
    }
}