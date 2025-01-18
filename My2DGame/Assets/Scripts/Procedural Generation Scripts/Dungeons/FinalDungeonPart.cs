using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalDungeonPart : MonoBehaviour
{
    public BoundsInt Bounds { get; private set; }
    public Vector2Int MonumentPosition { get; private set; }
    public Item Monument { get; private set; }
    public ColorCore ColorCore { get;  set; }
    public DungeonColor Color { get; private set; }

    public FinalDungeonPart(BoundsInt bounds, DungeonColor color)
    {
        Bounds = bounds;
        Color = color;
    }

    public void SetMonument(Vector2Int position, Item monument)
    {
        MonumentPosition = position;
        Monument = monument;
    }
}
