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
using UnityEngine.Tilemaps;

public class DungeonGenerator : MonoBehaviour, IDungeonGenerator
{
    [SerializeField]
    public Player Player;

    public Dungeon PinkDungeon { get; set; }

    public Dungeon BlueDungeon { get; set; }

    public Dungeon GreenDungeon { get; set; }

    public Dungeon PurpleDungeon { get; set; }

    public Tile greenTile;

    public Tile blueTile;

    private List<Character> allEnemiesList = new List<Character>();

    private List<Item> allItems = new List<Item>();

    private List<Vector2Int> occupiedPositions = new List<Vector2Int>();

    [SerializeField]
    protected RandomWalkParameters pinkDungeonParameters;

    [SerializeField]
    protected RandomWalkParameters blueDungeonParameters;

    [SerializeField]
    protected RandomWalkParameters greenDungeonParameters;

    [SerializeField]
    protected RandomWalkParameters purpleDungeonParameters;

    [SerializeField]
    protected MapCreator tileMap = null;

    [SerializeField]
    protected Vector2Int startPosition = Vector2Int.zero;

    [SerializeField]
    protected Vector3Int mapSize = new Vector3Int(400, 400, 0);

    [SerializeField]
    private int dungeonsCount = 4;

    [SerializeField]
    private int dungeonWidth = 120;

    [SerializeField]
    private int dungeonHeight = 120;

    [SerializeField]
    private int minRoomWidth = 50;

    [SerializeField]
    private int minRoomHeight = 50;

    [SerializeField]
    private int undergoundHeight = 10;

    [SerializeField]
    private int undergoundWidth = 20;

    [SerializeField]
    private int startX = 500;

    [SerializeField]
    private int undergroundSmoothness = 10;

    [SerializeField]
    private int modifier = 10;

    [SerializeField]
    private int seed;

    [SerializeField]
    [Range(0, 10)]
    private int offset = 1;

    [SerializeField]
    PinkDungeon pinkDungeonBehaviour;

    [SerializeField]
    BlueDungeon blueDungeonBehaviour;

    [SerializeField]
    GreenDungeon greenDungeonBehaviour;

    [SerializeField]
    PurpleDungeon purpleDungeonBehaviour;

    [SerializeField]
    private FinalLevelGenerator finalLevel;

    public Underground undergroundDungeon;

    public UndergroundBehaviour undergroundBeh;

    public void GenerateDungeons()
    {
        seed = UnityEngine.Random.Range(-10000, 10000);
        tileMap.ClearGeneration();
        ClearGeneratedObjects();
        allEnemiesList.Clear();
        allItems.Clear();
        CreateDungeons();
        pinkDungeonBehaviour.Create();
        blueDungeonBehaviour.Create();
        greenDungeonBehaviour.Create();
        purpleDungeonBehaviour.Create();
        ConnectDungeonsWithTeleports();
        CreateUnderground();
        CreateFinalLevel();
    }

    private void ConnectDungeonsWithTeleports()
    {
        List<Item> portals = new List<Item>();

        if (pinkDungeonBehaviour.portal != null) portals.Add(pinkDungeonBehaviour.portal);
        if (pinkDungeonBehaviour.portal2 != null) portals.Add(pinkDungeonBehaviour.portal2);

        if (blueDungeonBehaviour.portal != null) portals.Add(blueDungeonBehaviour.portal);
        if (blueDungeonBehaviour.portal2 != null) portals.Add(blueDungeonBehaviour.portal2);

        if (greenDungeonBehaviour.portal != null) portals.Add(greenDungeonBehaviour.portal);
        if (greenDungeonBehaviour.portal2 != null) portals.Add(greenDungeonBehaviour.portal2);

        if (purpleDungeonBehaviour.portal != null) portals.Add(purpleDungeonBehaviour.portal);
        if (purpleDungeonBehaviour.portal2 != null) portals.Add(purpleDungeonBehaviour.portal2);

        ShuffleList(portals);

        for (int i = 0; i < portals.Count - 1; i++)
        {
            var portalA = portals[i];
            var dungeonA = FindDungeonByPortal(portalA);

            for (int j = i + 1; j < portals.Count; j++)
            {
                var portalB = portals[j];
                var dungeonB = FindDungeonByPortal(portalB);

                if (dungeonA != dungeonB)
                {
                    var behaviourA = portalA.GetComponent<PortalBehaviour>();
                    var behaviourB = portalB.GetComponent<PortalBehaviour>();

                    behaviourA.TargetDungeon = dungeonB;
                    behaviourA.TargetPosition = portalB.transform;

                    behaviourB.TargetDungeon = dungeonA;
                    behaviourB.TargetPosition = portalA.transform;

                    portals.RemoveAt(j);
                    portals.RemoveAt(i);
                    i--;
                    break;
                }
            }
        }

    }

    private DungeonBehaviour FindDungeonByPortal(Item portal)
    {
        if (pinkDungeonBehaviour.portal == portal || pinkDungeonBehaviour.portal2 == portal) return pinkDungeonBehaviour;
        if (blueDungeonBehaviour.portal == portal || blueDungeonBehaviour.portal2 == portal) return blueDungeonBehaviour;
        if (greenDungeonBehaviour.portal == portal || greenDungeonBehaviour.portal2 == portal) return greenDungeonBehaviour;
        if (purpleDungeonBehaviour.portal == portal || purpleDungeonBehaviour.portal2 == portal) return purpleDungeonBehaviour;
        return null;
    }

    private void ShuffleList<T>(List<T> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int j = UnityEngine.Random.Range(0, i + 1);
            T temp = list[i];
            list[i] = list[j];
            list[j] = temp;
        }
    }

    private bool AlreadyLinked(DungeonBehaviour a, DungeonBehaviour b)
    {
        bool portal1Linked = a.portal != null && a.portal.GetComponent<PortalBehaviour>().TargetDungeon == b;
        bool portal2Linked = a.portal2 != null && a.portal2.GetComponent<PortalBehaviour>().TargetDungeon == b;
        return portal1Linked || portal2Linked;
    }



    private void CreateDungeons()
    {
        int maxAttempts = 10; // Maximální poèet pokusù pøed zvìtšením mapy
        int maxSizeIncrements = 10; // Kolikrát mùžeme zvìtšit mapu
        int sizeIncrement = 10; // O kolik zvìtšíme mapu pøi každém neúspìchu

        int mapSizeX = mapSize.x;
        int mapSizeY = mapSize.y;

        HashSet<BoundsInt> dungeonList = new HashSet<BoundsInt>();

        for (int i = 0; i < maxSizeIncrements; i++)
        {
            for (int attempts = 0; attempts < maxAttempts; attempts++)
            {
                dungeonList = ProceduralGenerationAlgorithms.BSP(
                    new BoundsInt(Vector3Int.zero, new Vector3Int(mapSizeX, mapSizeY, 0)),
                    dungeonWidth,
                    dungeonHeight,
                    dungeonsCount
                );


                if (dungeonList.Count >= dungeonsCount - 1)
                {
                    break;
                }
            }

            if (dungeonList.Count >= dungeonsCount - 1)
            {
                break;
            }

            mapSizeX += sizeIncrement;
            mapSizeY += sizeIncrement;
            Debug.LogWarning($"Zvìtšuji mapu na {mapSizeX}x{mapSizeY} pro lepší generování dungeonù.");
        }

        if (dungeonList.Count < 5)
        {
            Debug.LogError("Nepodaøilo se vygenerovat minimálnì 5 dungeonù ani po zvìtšení mapy!");
        }

        List<BoundsInt> dungeons = new List<BoundsInt>(dungeonList)
            .OrderByDescending(bounds => bounds.size.x * bounds.size.y)
            .ToList();

        List<Dungeon> createdDungeonsList = new List<Dungeon>();

        if (dungeons.Count > 0)
        {
            GreenDungeon = new Dungeon(dungeons[0], DungeonColor.Green);
            GreenDungeon.parameters = greenDungeonParameters;
            InitDungeon(GreenDungeon);
            createdDungeonsList.Add(GreenDungeon);
            dungeons.RemoveAt(0);
        }

        if (dungeons.Count > 0)
        {
            PinkDungeon = new Dungeon(dungeons[0], DungeonColor.Pink);
            PinkDungeon.parameters = pinkDungeonParameters;
            InitDungeon(PinkDungeon);
            createdDungeonsList.Add(PinkDungeon);
            dungeons.RemoveAt(0);
        }

        if (dungeons.Count > 0)
        {
            BlueDungeon = new Dungeon(dungeons[0], DungeonColor.Blue);
            BlueDungeon.parameters = blueDungeonParameters;
            InitDungeon(BlueDungeon);
            createdDungeonsList.Add(BlueDungeon);
            dungeons.RemoveAt(0);
        }

        if (dungeons.Count > 0)
        {
            PurpleDungeon = new Dungeon(dungeons[0], DungeonColor.Purple);
            PurpleDungeon.parameters = purpleDungeonParameters;
            InitDungeon(PurpleDungeon);
            createdDungeonsList.Add(PurpleDungeon);
            dungeons.RemoveAt(0);
        }

        if (PinkDungeon != null) tileMap.DrawFloor(PinkDungeon);
        if (BlueDungeon != null) tileMap.DrawFloor(BlueDungeon);
        if (GreenDungeon != null) tileMap.DrawFloor(GreenDungeon);
        if (PurpleDungeon != null) tileMap.DrawFloor(PurpleDungeon);

        if (PinkDungeon != null) WallGenerator.CreateAndDrawWalls(PinkDungeon, tileMap);
        if (BlueDungeon != null) WallGenerator.CreateAndDrawWalls(BlueDungeon, tileMap);
        if (GreenDungeon != null) WallGenerator.CreateAndDrawWalls(GreenDungeon, tileMap);
        if (PurpleDungeon != null) WallGenerator.CreateAndDrawWalls(PurpleDungeon, tileMap);

        DrawDungeonBounds(dungeonList.ToList());
    }

    private void DrawDungeonBounds(List<BoundsInt> dungeons)
    {
        foreach (var bounds in dungeons)
        {
            Vector3 bottomLeft = new Vector3(bounds.min.x, bounds.min.y, 0);
            Vector3 bottomRight = new Vector3(bounds.max.x, bounds.min.y, 0);
            Vector3 topLeft = new Vector3(bounds.min.x, bounds.max.y, 0);
            Vector3 topRight = new Vector3(bounds.max.x, bounds.max.y, 0);

            // Draw lines to form the rectangle
            Debug.DrawLine(bottomLeft, bottomRight, UnityEngine.Color.red, 10f); // Bottom
            Debug.DrawLine(bottomRight, topRight, UnityEngine.Color.red, 10f);  // Right
            Debug.DrawLine(topRight, topLeft, UnityEngine.Color.red, 10f);      // Top
            Debug.DrawLine(topLeft, bottomLeft, UnityEngine.Color.red, 10f);    // Left
        }
    }

    private void CreateFinalLevel()
    {
        finalLevel.GenerateFinalLevel();
    }

    public void CreateUnderground()
    {
        undergroundDungeon = new Underground();
        ProceduralGenerationAlgorithms.PerlinNoise(undergroundDungeon, undergoundHeight, undergoundWidth, undergroundSmoothness, modifier, startX);
        undergroundDungeon.Height = undergoundHeight;
        undergroundDungeon.Width = undergoundWidth;
        tileMap.DrawUndergroundFloor(undergroundDungeon);
    }

    public Vector2Int SetToRandomPositionInRandomRoom(Transform transformObject, Dungeon dungeon, int offset)
    {
        for (int attempt = 0; attempt < 100; attempt++)
        {
            int randomRoom = UnityEngine.Random.Range(0, dungeon.RoomList.Count);
            var room = dungeon.RoomList[randomRoom].FloorList.ToList();

            var newPos = room[UnityEngine.Random.Range(0 + offset, room.Count - offset)];

            if (IsPositionAndItsSurroundingsInList(room, newPos, offset) && !this.occupiedPositions.Contains(newPos))
            {
                transformObject.position = new Vector2 (newPos.x + 0.5f, newPos.y + 0.5f);
                return newPos;
            }
        }

        Debug.LogWarning("Nepodaøilo se najít platnou náhodnou pozici.");
        return Vector2Int.zero;
    }

    public float SetItemToRoomPosition(Item item, Room room, int width, int height, int offset, int spacing = 2)
    {
        float occupancy = (float)occupiedPositions.Count(pos => room.FloorList.Contains(pos)) / room.FloorList.Count;

        var grid = room.FloorList
            .Where(pos => IsValidPositionWithOffset(pos, room, offset))
            .OrderBy(_ => UnityEngine.Random.value)
            .ToList();

        if (grid.Count == 0)
        {
            Debug.LogWarning("Žádné volné pozice nejsou k dispozici.");
            return 1f;
        }

        foreach (var newPos in grid)
        {
            if (CanPlaceWithSpacing(newPos, room, width, height, spacing))
            {
                item.transform.position = new Vector2(newPos.x + 0.5f, newPos.y + 0.5f);
                item.Position = newPos;

                var newOccupiedPositions = GetOccupiedPositionsForLargeObject(newPos, width, height);
                foreach (var pos in newOccupiedPositions)
                {
                    occupiedPositions.Add(pos);
                    //tileMap.DrawTile(blueTile, pos); 
                }

                allItems.Add(item);
                return occupancy;
            }
        }

        Debug.LogWarning("Nepodaøilo se najít platnou pozici v místnosti.");
        return occupancy;
    }

    public void SetOverlapItemToRoomPosition(Item item, Room room, int offset)
    {
        var randomPosition = room.FloorList
            .Where(pos => IsValidPositionWithOffset(pos, room, offset))
            .OrderBy(_ => UnityEngine.Random.value)
            .FirstOrDefault();

        if (randomPosition != default)
        {
            item.transform.position = new Vector2(randomPosition.x + 0.5f, randomPosition.y + 0.5f);
            item.Position = randomPosition;

            allItems.Add(item);

            //Debug.Log($"Item {item.name} byl umístìn do místnosti na pozici {randomPosition}.");
        }
        else
        {
            Debug.LogWarning("Nebyla nalezena žádná platná pozice pro položku.");
        }
    }


    private bool CanPlaceWithSpacing(Vector2Int position, Room room, int width, int height, int spacing)
    {
        for (int x = -spacing; x <= spacing; x++)
        {
            for (int y = -spacing; y <= spacing; y++)
            {
                var checkPos = new Vector2Int(position.x + x, position.y + y);

                // Pokud je pozice obsazená nebo není souèástí podlahy, nelze umístit
                if (occupiedPositions.Contains(checkPos) || !room.FloorList.Contains(checkPos))
                {
                    return false;
                }
            }
        }
        return true;
    }


    private bool IsValidPositionWithOffset(Vector2Int position, Room room, int offset)
    {
        for (int x = -offset; x <= offset; x++)
        {
            for (int y = -offset; y <= offset; y++)
            {
                var checkPos = new Vector2Int(position.x + x, position.y + y);

                if (!room.FloorList.Contains(checkPos))
                {
                    return false;
                }
            }
        }

        return true;
    }


    private bool CanPlaceObject(Vector2Int position, Room room, int width, int height)
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                var checkPos = new Vector2Int(position.x + x, position.y + y);

                // Ovìøení, zda pozice je v místnosti a není obsazená
                if (!room.FloorList.Contains(checkPos) || occupiedPositions.Contains(checkPos))
                {
                    return false;
                }
            }
        }
        return true;
    }


    public List<Vector2Int> SetCharacterToRandomPositionInRandomRoom(Transform transformObject, Dungeon dungeon, int width, int height)
    {
        for (int attempt = 0; attempt < 100; attempt++)
        {
            int randomRoom = UnityEngine.Random.Range(0, dungeon.RoomList.Count);

            Room room = dungeon.RoomList[randomRoom];
            var roomList = room.FloorList.ToList();

            var basePos = roomList[UnityEngine.Random.Range(0 + offset, roomList.Count - offset)];

            var occupiedPositions = GetOccupiedPositionsForLargeObject(basePos, width, height);

            if (occupiedPositions.All(pos => roomList.Contains(pos) && !this.occupiedPositions.Contains(pos)))
            {
                transformObject.position = new Vector2(basePos.x + 0.5f, basePos.y + 0.5f);
                occupiedPositions.Add(basePos);
                return occupiedPositions;
            }
        }

        Debug.LogWarning("Nepodaøilo se najít volnou pozici pro character.");
        return null;
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

            var occupiedPositions = GetOccupiedPositionsForLargeObject(basePos, width, height);

            if (occupiedPositions.All(pos => room.Contains(pos) && !this.occupiedPositions.Contains(pos)))
            {
                transformObject.position = new Vector2(basePos.x + 0.5f, basePos.y + 0.5f);
                itemsOccupiedPositions.Add(basePos);
                occupiedPositions.Add(basePos);
                itemsOccupiedPositions.Union(occupiedPositions);
                return occupiedPositions;
            }
        }

        Debug.LogWarning("Nepodaøilo se najít volnou pozici pro velký objekt.");
        return itemsOccupiedPositions;
    }

    public List<Vector2Int> SetLargeObjectToRoomCenter(Transform transformObject, Dungeon dungeon, int width, int height)
    {
        for (int attempt = 0; attempt < 100; attempt++)
        {
            int randomRoom = UnityEngine.Random.Range(0, dungeon.RoomList.Count);

            Room room = dungeon.RoomList[randomRoom];
            var roomList = room.FloorList.ToList();

            var basePos = room.Center;

            var occupiedPositions = GetOccupiedPositionsForLargeObject(basePos, width, height);

            if (occupiedPositions.All(pos => roomList.Contains(pos) && !this.occupiedPositions.Contains(pos)))
            {
                transformObject.position = new Vector2 (basePos.x + 0.5f, basePos.y + 0.5f);
                occupiedPositions.Add(basePos);
                return occupiedPositions;
            }
        }

        Debug.LogWarning("Nepodaøilo se najít volnou pozici pro velký objekt.");
        return null;
    }

    public List<Vector2Int> SetCharacterToCenterOfRoom(Transform transformObject, Dungeon dungeon, int width, int height)
    {
        for (int attempt = 0; attempt < 100; attempt++)
        {
            int randomRoom = UnityEngine.Random.Range(0, dungeon.RoomList.Count);

            Room room = dungeon.RoomList[randomRoom];
            var roomList = room.FloorList.ToList();

            var basePos = room.Center;

            var occupiedPositions = GetOccupiedPositionsForLargeObject(basePos, width, height);

            if (occupiedPositions.All(pos => roomList.Contains(pos) && !this.occupiedPositions.Contains(pos)))
            {
                transformObject.position = new Vector2(basePos.x + 0.5f, basePos.y + 0.5f);
                occupiedPositions.Add(basePos);
                return occupiedPositions;
            }
        }

        Debug.LogWarning("Nepodaøilo se najít volnou pozici pro character.");
        return null;
    }

    private List<Vector2Int> GetOccupiedPositionsForLargeObject(Vector2Int basePos, int width, int height)
    {
        var positions = new List<Vector2Int>();
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                positions.Add(new Vector2Int(basePos.x + x, basePos.y + y));
                positions.Add(new Vector2Int(basePos.x - x, basePos.y - y));
                positions.Add(new Vector2Int(basePos.x + x, basePos.y - y));
                positions.Add(new Vector2Int(basePos.x - x, basePos.y + y));

            }
        }
        return positions;
    }

    public void setBossToRandomRoom(Character character, Dungeon dungeon, int height, int width)
    {
        var positions = SetCharacterToCenterOfRoom(character.transform, dungeon, width, height);
        if(positions == null)
        {
            positions = SetLargeObjectToRandomPosition(character.transform, dungeon, width, height, 2);
        }
        character.Position = positions[0];
        allEnemiesList.Add(character);
        occupiedPositions = occupiedPositions.Union(positions).ToList();
    }

    public void setCharacterToRandomPosition(Character character, Dungeon dungeon, int offset)
    {
        var position = SetToRandomPositionInRandomRoom(character.transform, dungeon, offset);
        //tileMap.DrawTile(blueTile, position);
        character.Position = position;
        allEnemiesList.Add(character);
        occupiedPositions.Add(position);
    }

    public void setLargeCharacterToCenterOfRandomRoom(Character character, Dungeon dungeon, int height, int width)
    {
        var positions = SetCharacterToCenterOfRoom(character.transform, dungeon, width, height);
        //foreach (var position in positions)
        //{
        //    //tileMap.DrawTile(blueTile, position);
        //}
        character.Position = positions[0];
        allEnemiesList.Add(character);
        occupiedPositions = occupiedPositions.Union(positions).ToList();
    }

    public void setLargeCharacterToRandomRoom(Character character, Dungeon dungeon, int height, int width, int offset)
    {
        var positions = SetLargeObjectToRandomPosition(character.transform, dungeon, width, height, offset);
        //foreach (var position in positions)
        //{
        //    //tileMap.DrawTile(blueTile, position);
        //}
        character.Position = positions[0];
        allEnemiesList.Add(character);
        occupiedPositions = occupiedPositions.Union(positions).ToList();
    }

    public void SetItemToEdgeOfRoom(Item item, Dungeon dungeon)
    {
        var position = SetToEdgeOfRoom(item.transform, dungeon);
        //tileMap.DrawTile(blueTile, position);
        item.Position = position;
        occupiedPositions.Add(position);
        allItems.Add(item);
    }

    public void SetItemToRandomPosition(Item item, Dungeon dungeon, int offset)
    {
        var position = SetToRandomPositionInRandomRoom(item.transform, dungeon, offset);
        //tileMap.DrawTile(blueTile, position);
        item.Position = position;
        occupiedPositions.Add(position);
        allItems.Add(item);
    }

    public void SetLargeItemToRandomPosition(Item item, Dungeon dungeon, int width, int height, int offset)
    {
        var positions = SetLargeObjectToRandomPosition(item.transform, dungeon, width, height, offset);
        item.Position = positions[0];
        occupiedPositions = occupiedPositions.Union(positions).ToList();
        allItems.Add(item);
    }

    public void SetLargeItemToRoomCenter(Item item, Dungeon dungeon, int width, int height)
    {
        var positions = SetLargeObjectToRoomCenter(item.transform, dungeon, width, height);
        //foreach (var position in positions) 
        //{
        //    //tileMap.DrawTile(greenTile, position);
        //}

        if (positions != null)
        {
            item.Position = positions[0];
            occupiedPositions = occupiedPositions.Union(positions).ToList();
            allItems.Add(item);
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
        HashSet<BoundsInt> allRooms = ProceduralGenerationAlgorithms.BSP(
            new BoundsInt(dungeon.DungeonBounds.min, new Vector3Int(dungeon.DungeonBounds.size.x, dungeon.DungeonBounds.size.y, 0)),
            minRoomWidth,
            minRoomHeight,
            20
        );

        List<BoundsInt> roomList = allRooms.ToList();

        //int numberOfRoomsToUse = UnityEngine.Random.Range((roomList.Count + 1)/2, roomList.Count + 1);

        //List<BoundsInt> selectedRooms = roomList.OrderBy(x => UnityEngine.Random.value).Take(numberOfRoomsToUse).ToList();

        dungeon.Floor.RoomList = new HashSet<BoundsInt>(allRooms);

        DrawRoomBounds(dungeon.Floor.RoomList.ToList());
    }




    private void DrawRoomBounds(List<BoundsInt> roomList)
    {
        foreach (var room in roomList)
        {
            Vector3 bottomLeft = new Vector3(room.min.x, room.min.y, 0);
            Vector3 bottomRight = new Vector3(room.max.x, room.min.y, 0);
            Vector3 topLeft = new Vector3(room.min.x, room.max.y, 0);
            Vector3 topRight = new Vector3(room.max.x, room.max.y, 0);

            Debug.DrawLine(bottomLeft, bottomRight, UnityEngine.Color.yellow, 10f);
            Debug.DrawLine(bottomRight, topRight, UnityEngine.Color.yellow, 10f);
            Debug.DrawLine(topRight, topLeft, UnityEngine.Color.yellow, 10f);
            Debug.DrawLine(topLeft, bottomLeft, UnityEngine.Color.yellow, 10f);
        }
    }

    private void CreateDungeonFloor(Dungeon dungeon)
    {
        dungeon.RoomList = new List<Room>();

        HashSet<Vector2Int> floor = FloorGenerator.CreateRandomRooms(dungeon.RoomList, dungeon.Floor.RoomList.ToList(), dungeon.parameters, offset);

        //if (dungeon.Color != DungeonColor.Green)
        //{
        //    AddRandomHolesToFloor(floor, minClusters: 10, maxClusters: 25, minClusterSize: 1, maxClusterSize: 4);
        //}

        HashSet<Vector2Int> corridors = FloorGenerator.ConnectRooms(new List<Vector2Int>(dungeon.Floor.RoomCentersList));

        floor.UnionWith(corridors);

        //FloorGenerator.FillHoles(floor);

        dungeon.Floor.FloorList = floor;
    }

    private void AddRandomHolesToFloor(HashSet<Vector2Int> floor, int minClusters, int maxClusters, int minClusterSize, int maxClusterSize)
    {
        // Poèet shlukù dìr
        int clusterCount = UnityEngine.Random.Range(minClusters, maxClusters);

        List<Vector2Int> floorList = new List<Vector2Int>(floor);

        for (int i = 0; i < clusterCount; i++)
        {
            // Vyber náhodnou poèáteèní pozici pro shluk
            Vector2Int clusterCenter = floorList[UnityEngine.Random.Range(0, floorList.Count)];

            // Vytvoø díry okolo clusterCenter
            CreateClusterOfHoles(floor, clusterCenter, UnityEngine.Random.Range(minClusterSize, maxClusterSize));
        }
    }

    private void CreateClusterOfHoles(HashSet<Vector2Int> floor, Vector2Int center, int clusterSize)
    {
        for (int x = -clusterSize / 2; x <= clusterSize / 2; x++)
        {
            for (int y = -clusterSize / 2; y <= clusterSize / 2; y++)
            {
                Vector2Int holePosition = center + new Vector2Int(x, y);

                // Odstranìní pozice pouze pokud existuje v podlaze
                if (floor.Contains(holePosition))
                {
                    floor.Remove(holePosition);
                }
            }
        }
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
        HashSet<Vector2Int> firstDungeonCorridor = new HashSet<Vector2Int>();
        HashSet<Vector2Int> secondDungeonCorridor = new HashSet<Vector2Int>();
        for (int i = 0; i < connectingCorridor.Count; i++)
        {
            var corridorPoint = connectingCorridor.ToList()[i];
            bool isInFirstDungeon = ((corridorPoint.x < firstDungeon.DungeonBounds.xMax) && (corridorPoint.x > firstDungeon.DungeonBounds.xMin) &&
                (corridorPoint.y < firstDungeon.DungeonBounds.yMax) && (corridorPoint.y > firstDungeon.DungeonBounds.yMin));
            bool isInSecondDungeon = ((corridorPoint.x < secondDungeon.DungeonBounds.xMax) && (corridorPoint.x > secondDungeon.DungeonBounds.xMin) &&
                (corridorPoint.y < secondDungeon.DungeonBounds.yMax) && (corridorPoint.y > secondDungeon.DungeonBounds.yMin));
            if (isInFirstDungeon) 
            {
                firstDungeonCorridor.Add(corridorPoint);
            }
            else if (isInSecondDungeon)
            {
                secondDungeonCorridor.Add(corridorPoint);
            }
            if (!isInSecondDungeon && !isInFirstDungeon)
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
        int maxCorridorLength = 20;
        if (firstDungeonCorridor.Count > maxCorridorLength)
        {
            Vector2Int midPoint = firstDungeonCorridor.ToList()[firstDungeonCorridor.Count / 2];
            GenerateCircularRoom(midPoint, 5, firstDungeon);
        }
        if (secondDungeonCorridor.Count > maxCorridorLength)
        {
            Vector2Int midPoint = secondDungeonCorridor.ToList()[secondDungeonCorridor.Count / 2];
            GenerateCircularRoom(midPoint, 5, secondDungeon);
        }
        firstDungeon.Floor.FloorList.UnionWith(firstDungeonCorridor);
        secondDungeon.Floor.FloorList.UnionWith(secondDungeonCorridor);
        firstDungeon.connectedDungeons.Add(secondDungeon);
        secondDungeon.connectedDungeons.Add(firstDungeon);
    }

    private static List<Vector2Int> GenerateStraightPath(Vector2Int start, Vector2Int end)
    {
        List<Vector2Int> path = new List<Vector2Int>();
        Vector2Int current = start;

        while (current != end)
        {
            path.Add(current);

            if (current.x < end.x) current.x++;
            else if (current.x > end.x) current.x--;

            if (current.y < end.y) current.y++;
            else if (current.y > end.y) current.y--;
        }

        path.Add(end);
        return path;
    }



    private static void GenerateCircularRoom(Vector2Int center, int radius, Dungeon dungeon)
    {
        HashSet<Vector2Int> circularRoom = new HashSet<Vector2Int>();

        for (int x = -radius; x <= radius; x++)
        {
            for (int y = -radius; y <= radius; y++)
            {
                if (x * x + y * y <= radius * radius) // Rovnice kruhu
                {
                    Vector2Int roomPoint = new Vector2Int(center.x + x, center.y + y);
                    circularRoom.Add(roomPoint);
                }
            }
        }

        // Pøidáme místnost do dungeonu
        dungeon.Floor.FloorList.UnionWith(circularRoom);

        Debug.Log($"Vygenerována kruhová místnost na {center} s polomìrem {radius}");
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

    private void ClearGeneratedObjects()
    {
        for (int i = 0; i < allEnemiesList.Count; i++)
        {
            if (allEnemiesList[i] != null)
            {
                Debug.Log($"Mazání: {allEnemiesList[i].name}");
                DestroyImmediate(allEnemiesList[i].gameObject);
                allEnemiesList[i] = null;
            }
        }
        allEnemiesList.RemoveAll(enemy => enemy == null);

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
