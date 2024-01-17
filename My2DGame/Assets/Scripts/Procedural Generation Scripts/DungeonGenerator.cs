using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using System;
using System.Drawing;

public class DungeonGenerator : MonoBehaviour, IDungeonGenerator
{
    [SerializeField]
    protected RandomWalkParameters randomWalkParameters;

    [SerializeField]
    protected MapCreator tileMap = null;

    [SerializeField]
    protected Vector2Int startPosition = Vector2Int.zero;

    [SerializeField]
    protected Vector3Int mapSize = new Vector3Int(400, 400, 0);

    [SerializeField]
    private int dungeonWidth = 120;

    [SerializeField]
    private int dungeonHeight = 120;

    [SerializeField]
    private int minRoomWidth = 20;

    [SerializeField]
    private int minRoomHeight = 20;

    [SerializeField]
    [Range(0, 10)]
    private int offset = 1;

    public void GenerateDungeons()
    {
        tileMap.ClearGeneration();
        CreateDungeons();
    }

    private void CreateDungeons()
    {
        HashSet<BoundsInt> dungeonList = new HashSet<BoundsInt>();
        while (dungeonList.Count < 2)
        {
            dungeonList = ProceduralGenerationAlgorithms.BSP(new BoundsInt(Vector3Int.zero, new Vector3Int(mapSize.x, mapSize.y, 0)), dungeonWidth, dungeonHeight);
        }
        List<BoundsInt> dungeons = new List<BoundsInt>(dungeonList);

        IDungeon pinkDungeon = new PinkDungeon(dungeons[0]);
        InitDungeon(pinkDungeon);

        IDungeon blueDungeon = new BlueDungeon(dungeons[1]);
        InitDungeon(blueDungeon);

        ConnectDungeons(pinkDungeon, blueDungeon);

        tileMap.DrawFloor(pinkDungeon);
        tileMap.DrawFloor(blueDungeon);

        WallGenerator.CreateAndDrawWalls(pinkDungeon, tileMap);
        WallGenerator.CreateAndDrawWalls(blueDungeon, tileMap);
    }

    private void InitDungeon(IDungeon dungeon)
    {
        GetRoomsPositions(dungeon);
        GetRoomsCenters(dungeon);
        CreateDungeonFloor(dungeon);
    }

    private void GetRoomsPositions(IDungeon dungeon)
    {
        dungeon.Floor.RoomList = ProceduralGenerationAlgorithms.BSP(new BoundsInt(dungeon.DungeonBounds.min, new Vector3Int
            (dungeon.DungeonBounds.size.x, dungeon.DungeonBounds.size.y, 0)), minRoomWidth, minRoomHeight);
    }

    private void CreateDungeonFloor(IDungeon dungeon)
    {
        HashSet<Vector2Int> floor = FloorGenerator.CreateRandomRooms(dungeon.Floor.RoomList.ToList(), randomWalkParameters, offset);

        HashSet<Vector2Int> corridors = FloorGenerator.ConnectRooms(new List<Vector2Int>(dungeon.Floor.RoomCentersList));

        floor.UnionWith(corridors);

        FloorGenerator.FillHoles(floor);

        dungeon.Floor.FloorList = floor;
    }

    private void GetRoomsCenters(IDungeon dungeon)
    {
        List<Vector2Int> centerRoomList = new List<Vector2Int>();
        foreach (var room in dungeon.Floor.RoomList)
        {
            centerRoomList.Add((Vector2Int)Vector3Int.RoundToInt(room.center));
        }
        dungeon.Floor.RoomCentersList = centerRoomList;
    }

    private void ConnectDungeons(IDungeon firstDungeon, IDungeon secondDungeon)
    {
        var firstDungeonCenters = firstDungeon.Floor.RoomCentersList;
        var secondDungeonCenters = secondDungeon.Floor.RoomCentersList;
        var currentRoomCenter = firstDungeonCenters[0];
        firstDungeonCenters.Remove(currentRoomCenter);
        float minDistance = float.MaxValue;
        Vector2Int minDistancePoint1 = Vector2Int.zero;
        Vector2Int minDistancePoint2 = Vector2Int.zero;
        for(int i = 0; i < firstDungeonCenters.Count; i++){
            currentRoomCenter = firstDungeonCenters[i];
            Vector2Int currentClosestPoint = FloorGenerator.FindClosestPoint(currentRoomCenter, secondDungeonCenters);
            float distance = Vector2Int.Distance(currentRoomCenter, currentClosestPoint);
            if (minDistance > distance)
            {
                minDistance = distance;
                minDistancePoint1 = currentRoomCenter;
                minDistancePoint2 = currentClosestPoint;
            }
        }
        HashSet<Vector2Int> connectingCorridor = FloorGenerator.CreateCorridor(minDistancePoint1, minDistancePoint2);
        int middleIndex = (connectingCorridor.Count) / 2; //11/2 = 5         0 1 2 3 4 5 6 7 8 9 10
        firstDungeon.Floor.FloorList.UnionWith(connectingCorridor.Take(middleIndex));
        secondDungeon.Floor.FloorList.UnionWith(connectingCorridor.Skip(middleIndex));
        firstDungeon.Floor.AnotherDungeonsEntrances.Add(connectingCorridor.ToList()[middleIndex]);
        secondDungeon.Floor.AnotherDungeonsEntrances.Add(connectingCorridor.ToList()[middleIndex-1]);
    }

}
