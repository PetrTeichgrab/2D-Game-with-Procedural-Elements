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


    private List<Character> allEnemiesList = new List<Character>();

    private List<Item> allItems = new List<Item>();

    private List<Vector2Int> occupiedPositions = new List<Vector2Int>();

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

    [SerializeField]
    PinkDungeon pinkDungeon;

    [SerializeField]
    BlueDungeon blueDungeon;

    [SerializeField]
    GreenDungeon greenDungeon;

    //public void Start()
    //{
    //    tileMap.ClearGeneration();
    //    CreateDungeons();
    //}
    public void GenerateDungeons()
    {
        tileMap.ClearGeneration();
        ClearGeneratedObjects();
        allEnemiesList.Clear();
        allItems.Clear();
        CreateDungeons();
        pinkDungeon.Create();
        //blueDungeon.Create();
        //greenDungeon.Create();
    }

    private void CreateDungeons()
    {
        HashSet<BoundsInt> dungeonList = new HashSet<BoundsInt>();
        while (dungeonList.Count < 5)
        {
            dungeonList = ProceduralGenerationAlgorithms.BSP(new BoundsInt(Vector3Int.zero, new Vector3Int(mapSize.x, mapSize.y, 0)), dungeonWidth, dungeonHeight);
        }
        List<BoundsInt> dungeons = new List<BoundsInt>(dungeonList);

        List<Dungeon> createdDungeonsList = new List<Dungeon>();

        PinkDungeon = new Dungeon(dungeons[0], DungeonColor.Pink);
        InitDungeon(PinkDungeon);
        createdDungeonsList.Add(PinkDungeon);

        //BlueDungeon = new Dungeon(dungeons[1], DungeonColor.Blue);
        //InitDungeon(BlueDungeon);
        //createdDungeonsList.Add(BlueDungeon);

        //GreenDungeon = new Dungeon(dungeons[2], DungeonColor.Green);
        //InitDungeon(GreenDungeon);
        //createdDungeonsList.Add(GreenDungeon);

        //PurpleDungeon = new Dungeon(dungeons[3], DungeonColor.Purple);
        //InitDungeon(PurpleDungeon);
        //createdDungeonsList.Add(PurpleDungeon);

        //for (int i = 0; i < createdDungeonsList.Count; i++)
        //{
        //    for (int j = createdDungeonsList.Count; j > 0; j--)
        //    {
        //        var NearestDungeon = FindNearestBounds(createdDungeonsList[i], createdDungeonsList);
        //        if (!createdDungeonsList[i].connectedDungeons.Contains(NearestDungeon))
        //        {
        //            ConnectDungeons(createdDungeonsList[i], NearestDungeon);
        //        }
        //    }
        //}

        tileMap.DrawFloor(PinkDungeon);
        //tileMap.DrawFloor(BlueDungeon);
        //tileMap.DrawFloor(GreenDungeon);
        //tileMap.DrawFloor(PurpleDungeon);


        WallGenerator.CreateAndDrawWalls(PinkDungeon, tileMap);
        //WallGenerator.CreateAndDrawWalls(BlueDungeon, tileMap);
        //WallGenerator.CreateAndDrawWalls(GreenDungeon, tileMap);
        //WallGenerator.CreateAndDrawWalls(PurpleDungeon, tileMap);

        SetToRandomPositionInRandomRoom(Player.transform, PinkDungeon, 1);
    }

    public Vector2Int SetToRandomPositionInRandomRoom(Transform transformObject, Dungeon dungeon, int offset)
    {
        for (int attempt = 0; attempt < 100; attempt++)
        {
            int randomRoom = UnityEngine.Random.Range(0, dungeon.RoomList.Count);
            var room = dungeon.RoomList[randomRoom].FloorList.ToList();

            var newPos = room[UnityEngine.Random.Range(0 + offset, room.Count - offset)];

            if (IsPositionAndItsSurroundingsInList(room, newPos, 3))
            {
                transformObject.position = (Vector2)newPos;
                return newPos;
            }
        }

        Debug.LogWarning("Nepodaøilo se najít platnou náhodnou pozici.");
        return Vector2Int.zero;
    }


    public Vector2Int SetToEdgeOfRoom(Transform transformObject, Dungeon dungeon)
    {
        var edgePositions = dungeon.Floor.Edges.ToList();
        if (edgePositions.Count == 0)
        {
            Debug.LogWarning("Nebyla nalezena žádná pozice na okraji místnosti.");
            return Vector2Int.zero;
        }

        for (int i = 0; i < edgePositions.Count; i++)
        {
            var newPos = edgePositions[UnityEngine.Random.Range(0, edgePositions.Count)];

            if (!GetAllOccupiedPositions().Contains(newPos))
            {
                Vector2 centeredPos = new Vector2(newPos.x + 0.5f, newPos.y + 0.5f);
                transformObject.position = centeredPos;
                return newPos;
            }
        }

        Debug.LogWarning("Všechny pozice na okraji místnosti jsou obsazené.");
        return Vector2Int.zero;
    }

    public List<Vector2Int> SetLargeObjectToRandomPosition(Transform transformObject, Dungeon dungeon, int width, int height, int offset)
    {
        List<Vector2Int> itemsOccupiedPositions= new List<Vector2Int>();
        for (int attempt = 0; attempt < 100; attempt++) // Limit pokusù
        {
            int randomRoom = UnityEngine.Random.Range(0, dungeon.RoomList.Count);
            var room = dungeon.RoomList[randomRoom].FloorList.ToList();

            var basePos = room[UnityEngine.Random.Range(0 + offset, room.Count - offset)];

            occupiedPositions = GetOccupiedPositionsForLargeObject(basePos, width, height);

            if (occupiedPositions.All(pos => room.Contains(pos) && !GetAllOccupiedPositions().Contains(pos)))
            {
                transformObject.position = (Vector2)basePos;
                itemsOccupiedPositions.Add(basePos);
                itemsOccupiedPositions.Union(occupiedPositions);
                return occupiedPositions;
            }
        }

        Debug.LogWarning("Nepodaøilo se najít volnou pozici pro velký objekt.");
        return itemsOccupiedPositions;
    }

    public List<Vector2Int> SetLargeObjectToRoomCenter(Transform transformObject, Dungeon dungeon, int width, int height)
    {
        List<Vector2Int> itemsOccupiedPositions = new List<Vector2Int>();
        for (int attempt = 0; attempt < 100; attempt++)
        {
            int randomRoom = UnityEngine.Random.Range(0, dungeon.RoomList.Count);

            Room room = dungeon.RoomList[randomRoom];
            var roomList = room.FloorList.ToList();

            var basePos = room.Center;

            occupiedPositions = GetOccupiedPositionsForLargeObject(basePos, width, height);

            if (occupiedPositions.All(pos => roomList.Contains(pos) && !GetAllOccupiedPositions().Contains(pos)))
            {
                transformObject.position = (Vector2)basePos;
                itemsOccupiedPositions.Add(basePos);
                itemsOccupiedPositions.Union(occupiedPositions);
                return occupiedPositions;
            }
        }

        Debug.LogWarning("Nepodaøilo se najít volnou pozici pro velký objekt.");
        return occupiedPositions;
    }

    private List<Vector2Int> GetOccupiedPositionsForLargeObject(Vector2Int basePos, int width, int height)
    {
        var positions = new List<Vector2Int>();
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                positions.Add(new Vector2Int(basePos.x + x, basePos.y + y));
            }
        }
        return positions;
    }

    public void setCharacterToRandomPosition(Character character, Dungeon dungeon, int offset)
    {
        var position = SetToRandomPositionInRandomRoom(character.transform, dungeon, offset);
        character.Position = position;
        allEnemiesList.Add(character);
        occupiedPositions.Add(position);
    }

    public void SetItemToEdgeOfRoom(Item item, Dungeon dungeon)
    {
        var position = SetToEdgeOfRoom(item.transform, dungeon);
        item.Position = position;
        occupiedPositions.Add(position);
        allItems.Add(item);
    }

    public void SetItemToRandomPosition(Item item, Dungeon dungeon, int offset)
    {
        var position = SetToRandomPositionInRandomRoom(item.transform, dungeon, offset);
        item.Position = position;
        occupiedPositions.Add(position);
        allItems.Add(item);
    }

    public void SetLargeItemToRandomPosition(Item item, Dungeon dungeon, int width, int height, int offset)
    {
        var positions = SetLargeObjectToRandomPosition(item.transform, dungeon, width, height, offset);
        item.Position = positions[0];
        occupiedPositions.Union(positions);
        allItems.Add(item);
    }

    public void SetLargeItemToRoomCenter(Item item, Dungeon dungeon, int width, int height)
    {
        var positions = SetLargeObjectToRoomCenter(item.transform, dungeon, width, height);
        item.Position = positions[0];
        occupiedPositions.Union(positions);
        allItems.Add(item);
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
        foreach (Character enemy in allEnemiesList)
        {
            if (enemy != null)
            {
                DestroyImmediate(enemy.gameObject);
            }
        }
        foreach (Item item in allItems)
        {
            if (item != null)
            {
                DestroyImmediate(item.gameObject);
            }
        }
    }

    public List<Vector2Int> GetAllOccupiedPositions()
    {
        List<Vector2Int> occupiedPositions = new List<Vector2Int>();

        foreach (var enemy in allEnemiesList)
        {
            occupiedPositions.Add(enemy.Position);
        }

        foreach (var item in allItems)
        {
            occupiedPositions.Add(item.Position);
        }

        return occupiedPositions;
    }

}
