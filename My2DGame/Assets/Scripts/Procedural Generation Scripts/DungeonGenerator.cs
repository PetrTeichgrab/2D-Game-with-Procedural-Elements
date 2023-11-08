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

    public void GenerateDungeons()
    {
        tileMap.ClearGeneration();
        PinkDungeon pinkDungeon = new PinkDungeon();
        GenerateOneColorDungeon(pinkDungeon);
        BlueDungeon blueDungeon = new BlueDungeon();
        blueDungeon.StartPosition = pinkDungeon.Floor.floorList.Last() + new Vector2Int(150,150);
        GenerateOneColorDungeon(blueDungeon);
    }

    public void GenerateOneColorDungeon(IDungeon dungeon)
    {
        HashSet<Vector2Int> floor = dungeon.Floor.floorList;

        HashSet<Vector2Int> rooms = new HashSet<Vector2Int>();

        //Corridor creation
        FloorGenerator.CreateCorridors(floor, rooms, dungeon.StartPosition, amountOfCorridors, corridorLength);

        //Room creation
        FloorGenerator.CreateRooms(randomWalkParameters, rooms);

        floor.UnionWith(rooms);

        //Filling holes in floor
        FloorGenerator.FillHoles(floor);

        //FloorDrawing
        tileMap.DrawFloor(floor, dungeon.Color);

        //Creating and Drawing walls
        WallGenerator.CreateAndDrawWalls(floor, tileMap, dungeon.Color);
    }

}
