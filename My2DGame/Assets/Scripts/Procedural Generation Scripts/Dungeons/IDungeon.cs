using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDungeon
{
    public Walls Walls { get; set; }
    public Floor Floor { get; set; }
    public Color Color { get; }
    public BoundsInt DungeonBounds { get; set; }

}
