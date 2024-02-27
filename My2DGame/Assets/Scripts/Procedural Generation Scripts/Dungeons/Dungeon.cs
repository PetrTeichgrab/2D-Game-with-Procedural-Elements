using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dungeon
{
    public Walls Walls { get; set; }
    public Floor Floor { get; set; }
    public List<Room> RoomList { get; set; }
    public Color Color { get; }
    public BoundsInt DungeonBounds { get; set; }
    public Dungeon(BoundsInt bounds, Color color)
    {
        DungeonBounds = bounds;
        Color = color;
        Floor = new Floor();
        Walls = new Walls();
    }

}
