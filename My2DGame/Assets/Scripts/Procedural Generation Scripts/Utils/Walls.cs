using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Walls
{
    public HashSet<Vector2Int> CardinalWalls { get; set; } = new HashSet<Vector2Int>();

    public HashSet<Vector2Int> DiagonalWalls { get; set; } = new HashSet<Vector2Int>();

    public HashSet<Vector2Int> AllWalls { get; set; } = new HashSet<Vector2Int>();

    public void UpdateAll()
    {
        AllWalls.UnionWith(CardinalWalls);
        AllWalls.UnionWith(DiagonalWalls);
    }
}
