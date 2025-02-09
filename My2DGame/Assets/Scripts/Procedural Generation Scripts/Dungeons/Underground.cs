using System;
using System.Collections.Generic;
using UnityEngine;

public class Underground
{
    public HashSet<Vector2Int> Floor { get; set; } = new HashSet<Vector2Int>();
    public HashSet<Vector2Int> Cave { get; set; } = new HashSet<Vector2Int>();
    public HashSet<Vector2Int> Boundaries { get; set; } = new HashSet<Vector2Int>();

    public int Height {  get; set; }
    public int Width { get; set; }
}
