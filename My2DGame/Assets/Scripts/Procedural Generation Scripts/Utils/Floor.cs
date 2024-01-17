using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floor
{
    public HashSet<Vector2Int> FloorList {  get; set; } = new HashSet<Vector2Int>();
    public HashSet<BoundsInt> RoomList { get; set; } = new HashSet<BoundsInt>();
    public List<Vector2Int> RoomCentersList { get; set; } = new List<Vector2Int>();

    public List<Vector2Int> AnotherDungeonsEntrances { get; set; } = new List<Vector2Int>();

}
