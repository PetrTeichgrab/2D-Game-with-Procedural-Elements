using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PinkDungeon : IDungeon
{
    public Walls Walls { get; set; }
    public Floor Floor { get; set; }
    public Color Color { get; }
    public Vector2Int StartPosition { get; set; }

    public PinkDungeon()
    {
        StartPosition = Vector2Int.zero;
        Color = Color.Pink;
        Floor = new Floor();
        Walls = new Walls();
    }
}
