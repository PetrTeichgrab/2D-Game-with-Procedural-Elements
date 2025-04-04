using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dungeon
{
    public Walls Walls { get; set; }
    public Floor Floor { get; set; }
    public List<Room> RoomList { get; set; }
    public DungeonColor Color { get; }
    public BoundsInt DungeonBounds { get; set; }
    public List<Dungeon> connectedDungeons { get; set; } = new List<Dungeon>();
  
    public RandomWalkParameters parameters { get; set; }
    public Dungeon(BoundsInt bounds, DungeonColor color)
    {
        DungeonBounds = bounds;
        Color = color;
        Floor = new Floor();
        Walls = new Walls();
    }

}
