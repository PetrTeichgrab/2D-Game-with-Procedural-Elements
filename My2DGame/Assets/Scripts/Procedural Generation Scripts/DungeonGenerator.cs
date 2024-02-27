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

public class DungeonGenerator : MonoBehaviour, IDungeonGenerator
{
    [SerializeField]
    public Player Player;

    public EnemyMushroomPink PinkMushroom;

    public BlueSlime BlueSlime;

    public Dungeon PinkDungeon { get; set; }

    public Dungeon BlueDungeon { get; set; }

    private List<Enemy> enemyList = new List<Enemy>();

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
        while (dungeonList.Count < 2)
        {
            dungeonList = ProceduralGenerationAlgorithms.BSP(new BoundsInt(Vector3Int.zero, new Vector3Int(mapSize.x, mapSize.y, 0)), dungeonWidth, dungeonHeight);
        }
        List<BoundsInt> dungeons = new List<BoundsInt>(dungeonList);

        PinkDungeon = new Dungeon(dungeons[0], Color.Pink);
        InitDungeon(PinkDungeon);

        BlueDungeon = new Dungeon(dungeons[1], Color.Blue);
        InitDungeon(BlueDungeon);

        ConnectDungeons(PinkDungeon, BlueDungeon);

        tileMap.DrawFloor(PinkDungeon);
        tileMap.DrawFloor(BlueDungeon);

        WallGenerator.CreateAndDrawWalls(PinkDungeon, tileMap);
        WallGenerator.CreateAndDrawWalls(BlueDungeon, tileMap);

        SetCharactersPositions();

    }

    private void SetCharactersPositions()
    {
        for (int i = 0; i < 25; i++)
        {
            EnemyMushroomPink pinkMushroom = Instantiate(PinkMushroom, PinkMushroom.transform.position, PinkMushroom.transform.rotation);
            SetToRandomPositionInRandomRoom(pinkMushroom.transform, PinkDungeon, 1);
            enemyList.Add(pinkMushroom);
            BlueSlime blueSlime = Instantiate(BlueSlime, BlueSlime.transform.position, BlueSlime.transform.rotation);
            SetToRandomPositionInRandomRoom(blueSlime.transform, BlueDungeon, 1);
            enemyList.Add(blueSlime);
        }
        SetToRandomPositionInRandomRoom(Player.transform, BlueDungeon, 1);
    }

    private void SetToRandomPositionInRandomRoom(Transform transformObject, Dungeon dungeon, int offset)
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

    private void ClearGeneratedObjects() {
        foreach (Enemy enemy in enemyList)
        {
            if (enemy != null)
            {
                Destroy(enemy.gameObject);
            }
        }
    }

}
