using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using System;
using System.Drawing;
using System.Linq.Expressions;
using JetBrains.Annotations;
using UnityEngine.XR;

public class DungeonGenerator : MonoBehaviour, IDungeonGenerator
{
    [SerializeField]
    public Player Player;

    public EnemyMushroomPink PinkMushroom;

    public BlueSlime BlueSlime;

    public Dungeon PinkDungeon { get; set; }

    public Dungeon BlueDungeon { get; set; }

    public Dungeon GreenDungeon { get; set; }

    public Dungeon PurpleDungeon { get; set; }


    public List<Character> allEnemiesList = new List<Character>();

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

    //public void Start()
    //{
    //    tileMap.ClearGeneration();
    //    CreateDungeons();
    //}
    public void GenerateDungeons()
    {
        tileMap.ClearGeneration();
        ClearGeneratedObjects();
        CreateDungeons();
    }

    private void CreateDungeons()
    {
        HashSet<BoundsInt> dungeonList = new HashSet<BoundsInt>();
        while (dungeonList.Count < 4)
        {
            dungeonList = ProceduralGenerationAlgorithms.BSP(new BoundsInt(Vector3Int.zero, new Vector3Int(mapSize.x, mapSize.y, 0)), dungeonWidth, dungeonHeight);
        }
        List<BoundsInt> dungeons = new List<BoundsInt>(dungeonList);

        List<Dungeon> createdDungeonsList = new List<Dungeon>();

        PinkDungeon = new Dungeon(dungeons[0], DungeonColor.Pink);
        InitDungeon(PinkDungeon);
        createdDungeonsList.Add(PinkDungeon);

        BlueDungeon = new Dungeon(dungeons[1], DungeonColor.Blue);
        InitDungeon(BlueDungeon);
        createdDungeonsList.Add(BlueDungeon);

        GreenDungeon = new Dungeon(dungeons[2], DungeonColor.Green);
        InitDungeon(GreenDungeon);
        createdDungeonsList.Add(GreenDungeon);

        PurpleDungeon = new Dungeon(dungeons[3], DungeonColor.Purple);
        InitDungeon(PurpleDungeon);
        createdDungeonsList.Add(PurpleDungeon);

        for (int i = 0; i < createdDungeonsList.Count; i++ )
        {
            var NearestDungeon = FindNearestBounds(createdDungeonsList[i], createdDungeonsList);
            if (!createdDungeonsList[i].connectedDungeons.Contains(NearestDungeon))
            {
                ConnectDungeons(createdDungeonsList[i], NearestDungeon);
            }
        }

        tileMap.DrawFloor(PinkDungeon);
        tileMap.DrawFloor(BlueDungeon);
        tileMap.DrawFloor(GreenDungeon);
        tileMap.DrawFloor(PurpleDungeon);


        WallGenerator.CreateAndDrawWalls(PinkDungeon, tileMap);
        WallGenerator.CreateAndDrawWalls(BlueDungeon, tileMap);
        WallGenerator.CreateAndDrawWalls(GreenDungeon, tileMap);
        WallGenerator.CreateAndDrawWalls(PurpleDungeon, tileMap);


        SetToRandomPositionInRandomRoom(Player.transform, BlueDungeon, 1);
    }

    public void SetToRandomPositionInRandomRoom(Transform transformObject, Dungeon dungeon, int offset)
    {
        int randomRoom = UnityEngine.Random.Range(0, dungeon.RoomList.Count);
        var room = dungeon.RoomList[randomRoom].FloorList.ToList();
        var newPos = room[UnityEngine.Random.Range(0 + offset, room.Count - offset)];
        if(!IsPositionAndItsSurroundingsInList(room, newPos, 3))
        {
            SetToRandomPositionInRandomRoom(transformObject, dungeon, offset);
        }
        else
        {
            transformObject.position = (Vector2)newPos;
        }
    }

    bool IsPositionAndItsSurroundingsInList(List<Vector2Int> list, Vector2Int position, int offset)
    {
        if (list.Contains(position))
        {
            foreach (Vector2Int direction in Directions.AllDirectionsDic.Values)
            {
                var surroundingPos = position;

                for (int i = 0; i < offset; i++)
                {
                    surroundingPos += direction;
                    if (!list.Contains(surroundingPos))
                    {
                        return false;
                    }
                }
            }
            return true;
        }
        else
        {
            return false;
        }
    }

    private void InitDungeon(Dungeon dungeon)
    {
        GetRoomsPositions(dungeon);
        GetRoomsCenters(dungeon);
        CreateDungeonFloor(dungeon);
    }

    private void GetRoomsPositions(Dungeon dungeon)
    {
        dungeon.Floor.RoomList = ProceduralGenerationAlgorithms.BSP(new BoundsInt(dungeon.DungeonBounds.min, new Vector3Int
            (dungeon.DungeonBounds.size.x, dungeon.DungeonBounds.size.y, 0)), minRoomWidth, minRoomHeight);
    }

    private void CreateDungeonFloor(Dungeon dungeon)
    {
        dungeon.RoomList = new List<Room>();

        HashSet<Vector2Int> floor = FloorGenerator.CreateRandomRooms(dungeon.RoomList, dungeon.Floor.RoomList.ToList(), randomWalkParameters, offset);

        HashSet<Vector2Int> corridors = FloorGenerator.ConnectRooms(new List<Vector2Int>(dungeon.Floor.RoomCentersList));

        floor.UnionWith(corridors);

        //FloorGenerator.FillHoles(floor);

        dungeon.Floor.FloorList = floor;
    }

    private void GetRoomsCenters(Dungeon dungeon)
    {
        List<Vector2Int> centerRoomList = new List<Vector2Int>();
        foreach (var room in dungeon.Floor.RoomList)
        {
            centerRoomList.Add((Vector2Int)Vector3Int.RoundToInt(room.center));
        }
        dungeon.Floor.RoomCentersList = centerRoomList;
    }

    private void ConnectDungeons(Dungeon firstDungeon, Dungeon secondDungeon)
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
            Vector2Int currentClosestPoint = FloorGenerator.FindNearestPoint(currentRoomCenter, secondDungeonCenters);
            float distance = Vector2Int.Distance(currentRoomCenter, currentClosestPoint);
            if (minDistance > distance)
            {
                minDistance = distance;
                minDistancePoint1 = currentRoomCenter;
                minDistancePoint2 = currentClosestPoint;
            }
        }
        HashSet<Vector2Int> connectingCorridor = FloorGenerator.CreateCorridor(minDistancePoint1, minDistancePoint2);
        for (int i = 0; i < connectingCorridor.Count; i++)
        {
            var corridorPoint = connectingCorridor.ToList()[i];
            bool isInFirstDungeon = ((corridorPoint.x < firstDungeon.DungeonBounds.xMax) && (corridorPoint.x > firstDungeon.DungeonBounds.xMin) &&
                (corridorPoint.y < firstDungeon.DungeonBounds.yMax) && (corridorPoint.y > firstDungeon.DungeonBounds.yMin));
            bool isInSecondDungeon = ((corridorPoint.x < secondDungeon.DungeonBounds.xMax) && (corridorPoint.x > secondDungeon.DungeonBounds.xMin) &&
                (corridorPoint.y < secondDungeon.DungeonBounds.yMax) && (corridorPoint.y > secondDungeon.DungeonBounds.yMin));
            if (isInFirstDungeon) 
            {
                firstDungeon.Floor.FloorList.Add(corridorPoint);
            }
            else if (isInSecondDungeon)
            {
                secondDungeon.Floor.FloorList.Add(corridorPoint);
            }
            if(!isInSecondDungeon && !isInFirstDungeon)
            {
                secondDungeon.Floor.FloorList.Add(corridorPoint);
                firstDungeon.Floor.AnotherDungeonsEntrances.Add(corridorPoint);
                firstDungeon.Floor.AnotherDungeonsEntrances.Add(connectingCorridor.ToList()[i+2]);
                firstDungeon.Floor.AnotherDungeonsEntrances.Add(connectingCorridor.ToList()[i+1]);
                secondDungeon.Floor.AnotherDungeonsEntrances.Add(connectingCorridor.ToList()[i - 1]);
                secondDungeon.Floor.AnotherDungeonsEntrances.Add(connectingCorridor.ToList()[i - 2]);
                secondDungeon.Floor.AnotherDungeonsEntrances.Add(connectingCorridor.ToList()[i - 3]);
            }
        }
        firstDungeon.connectedDungeons.Add(secondDungeon);
        secondDungeon.connectedDungeons.Add(firstDungeon);
    }

    public Dungeon FindNearestBounds(Dungeon targetDungeon, List<Dungeon> otherDungeons)
    {
        var otherDungeonsCopy = otherDungeons.Where((v, i) => i != otherDungeons.IndexOf(targetDungeon)).ToList();
        Dungeon nearestDungeon = otherDungeonsCopy[0];
        float shortestDistance = Vector3.Distance(targetDungeon.DungeonBounds.center, otherDungeonsCopy[0].DungeonBounds.center);

        foreach (Dungeon dungeon in otherDungeonsCopy)
        {
            float distance = Vector3.Distance(targetDungeon.DungeonBounds.center, dungeon.DungeonBounds.center);

            if (distance < shortestDistance)
            {
                shortestDistance = distance;
                nearestDungeon = dungeon;
            }
        }

        return nearestDungeon;
    }

    private void ClearGeneratedObjects() {
        foreach (RangeEnemy enemy in allEnemiesList)
        {
            if (enemy != null)
            {
                Destroy(enemy.gameObject);
            }
        }
    }

}
