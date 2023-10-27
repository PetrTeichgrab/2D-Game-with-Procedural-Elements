using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class DungeonGenerator : MonoBehaviour, IDungeonGenerator
{
    [SerializeField]
    protected RandomWalkParameters randomWalkParameters;

    [SerializeField]
    protected MapCreator tileMap = null;

    [SerializeField]
    protected Vector2Int startPosition = Vector2Int.zero;

    //Pozdeji hodit do Parameters
    [SerializeField]
    private int corridorLength = 30;

    [SerializeField]
    private int amountOfCorridors = 20;

    public void GenerateDungeon()
    {
        tileMap.ClearGeneration();
        RunProceduralGeneration();
    }

    public void RunProceduralGeneration()
    {
        HashSet<Vector2Int> floor = new HashSet<Vector2Int>();

        HashSet<Vector2Int> rooms = new HashSet<Vector2Int>();

        //Corridor creation
        FloorGenerator.CreateCorridors(floor, rooms, startPosition, amountOfCorridors, corridorLength);

        //Room creation
        FloorGenerator.CreateRooms(randomWalkParameters, rooms);

        floor.UnionWith(rooms);

        //Filling holes in floor
        FloorGenerator.FillHoles(floor);

        //FloorDrawing
        tileMap.DrawFloor(floor);

        //Creating and Drawing walls
        WallGenerator.CreateAndDrawWalls(floor, tileMap);
    }
}
