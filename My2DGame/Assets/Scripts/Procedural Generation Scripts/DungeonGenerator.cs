using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using System;

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
        var dungeonList = BSP(new BoundsInt(new Vector3Int(0, 0, 0), new Vector3Int(1000, 1000, 0)), 200, 200);
        Debug.Log("Hello");
        Console.WriteLine(dungeonList.ToList().ToString());
        PinkDungeon pinkDungeon = new PinkDungeon(dungeonList[0]);
        GenerateOneColorDungeon(pinkDungeon);
        BlueDungeon blueDungeon = new BlueDungeon(dungeonList[1]);
        GenerateOneColorDungeon(blueDungeon);
    }

    public void GenerateOneColorDungeon(IDungeon dungeon)
    {
        HashSet<Vector2Int> floor = dungeon.Floor.floorList;

        HashSet<Vector2Int> rooms = new HashSet<Vector2Int>();

        //Corridor creation
        FloorGenerator.CreateCorridors(floor, rooms, amountOfCorridors, corridorLength, dungeon.DungeonBounds);

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

    public List<BoundsInt> BSP(BoundsInt spaceToSplit, int minWidth, int minHeight)
    {
        Queue<BoundsInt> dungeonsQueue = new Queue<BoundsInt>();
        List<BoundsInt> dungeonsList = new List<BoundsInt>();
        dungeonsQueue.Enqueue(spaceToSplit);

        while(dungeonsQueue.Count > 0)
        {
            var dungeon = dungeonsQueue.Dequeue();
            if (dungeon.size.y >= minHeight && dungeon.size.x >= minWidth)
            {
                if(UnityEngine.Random.value < 0.5f)
                {
                    if(dungeon.size.y >= minHeight * 2)
                    {
                        SplitHorizontally(minWidth, dungeonsQueue, dungeon);
                    }
                    else if(dungeon.size.x >= minWidth*2)
                    {
                        SplitVertically(minHeight, dungeonsQueue, dungeon);
                    }
                    else
                    {
                        dungeonsList.Add(dungeon);
                    }
                }
                else
                {
                    if (dungeon.size.x >= minWidth * 2)
                    {
                        SplitVertically(minWidth, dungeonsQueue, dungeon);
                    }
                    else if (dungeon.size.y >= minHeight * 2)
                    {
                        SplitHorizontally(minHeight, dungeonsQueue, dungeon);
                    }
                    else
                    {
                        dungeonsList.Add(dungeon);
                    }
                }
            }
        }
        return dungeonsList;
    }

    private void SplitVertically(int minWidth, Queue<BoundsInt> dungeonsQueue, BoundsInt dungeon)
    {
        var xSplit = UnityEngine.Random.Range(1, dungeon.size.x);
        BoundsInt dungeon1 = new BoundsInt(dungeon.min, new Vector3Int(xSplit, dungeon.min.y, dungeon.min.z));
        BoundsInt dungeon2 = new BoundsInt(new Vector3Int(dungeon.min.x + xSplit, dungeon.min.y, dungeon.min.z),
            new Vector3Int(dungeon.size.x - xSplit, dungeon.size.y, dungeon.size.z));
        dungeonsQueue.Enqueue(dungeon1);
        dungeonsQueue.Enqueue(dungeon2);
    }

    private void SplitHorizontally(int minHeight, Queue<BoundsInt> dungeonsQueue, BoundsInt dungeon)
    {
        var ySplit = UnityEngine.Random.Range(1, dungeon.size.y);
        BoundsInt dungeon1 = new BoundsInt(dungeon.min, new Vector3Int(dungeon.size.x, ySplit, dungeon.size.z));
        BoundsInt dungeon2 = new BoundsInt(new Vector3Int(dungeon.min.x, dungeon.min.y + ySplit, dungeon.min.z),
            new Vector3Int(dungeon.size.x, dungeon.size.y - ySplit, dungeon.size.z));
        dungeonsQueue.Enqueue(dungeon1);
        dungeonsQueue.Enqueue(dungeon2);
    }
}
