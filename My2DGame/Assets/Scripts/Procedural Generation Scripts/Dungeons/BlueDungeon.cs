using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BlueDungeon : IDungeon
{
    public Walls Walls { get; set; }

    public Floor Floor { get; set; }
    public Color Color { get; }

    public BoundsInt DungeonBounds { get; set; }

    public BlueDungeon(BoundsInt bounds)
    {
        DungeonBounds = bounds;
        Color = Color.Blue;
        Floor = new Floor();
        Walls = new Walls();
    }
}
