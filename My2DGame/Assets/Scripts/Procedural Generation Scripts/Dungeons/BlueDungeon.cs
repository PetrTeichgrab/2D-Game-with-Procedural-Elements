using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueDungeon : IDungeon
{
    public Walls Walls { get; set; }

    public Floor Floor { get; set; }
    public Color Color { get; }
    public Vector2Int StartPosition { get; set; }


    public BlueDungeon()
    {
        Color = Color.Blue;
        Floor = new Floor();
        Walls = new Walls();
    }
}
