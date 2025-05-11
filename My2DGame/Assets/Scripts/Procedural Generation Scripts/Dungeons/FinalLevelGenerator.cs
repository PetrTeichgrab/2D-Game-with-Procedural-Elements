using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

public class FinalLevelGenerator : MonoBehaviour
{
    [SerializeField]
    private int levelSize = 99;

    [SerializeField]
    private UnityEngine.Tilemaps.Tile greenFloorTile, pinkFloorTile, blueFloorTile, purpleFloorTile, emptyTile, floorTile, wallTile;

    [SerializeField]
    private Tilemap colliderMap;

    [SerializeField]
    private MapCreator mapCreator;

    [SerializeField]
    private Vector2Int startCoordinates = new Vector2Int(500, 500);

    [SerializeField]
    private List<Item> allItems = new List<Item>();

    [SerializeField]
    Player player;

    [SerializeField]
    ColorCore pinkColorCore, blueColorCore, greenColorCore, purpleColorCore;

    [SerializeField]
    Countdown countdown;

    private BoundsInt mazeBounds;
    private BoundsInt part1Bounds;
    private BoundsInt part2Bounds;
    private BoundsInt part3Bounds;
    private BoundsInt part4Bounds;

    private HashSet<Vector2Int> mazeFloor = new HashSet<Vector2Int>();

    [SerializeField]
    private Item monumentPrefab, firePedestalPrefab;

    [SerializeField]
    private Light2D globalLight;


    private List<FinalDungeonPart> dungeonParts = new List<FinalDungeonPart>();

    private AudioManager audioManager;

    private bool isTeleportedToFinalLevel;

    private float hintCooldown = 5f;
    private float nextHintTime = 0f;
    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
        isTeleportedToFinalLevel = false;
    }

    private void Update()
    {
        Vector2 playerPosition = player.transform.position;

        foreach (var part in dungeonParts)
        {
            if (Vector2.Distance(playerPosition, part.MonumentPosition) < 1.5f)
            {
                if (Time.time >= nextHintTime)
                {
                    AlertText.Instance.ShowAlert("USE [E] TO INPUT COLOR", 1.5f);
                    nextHintTime = Time.time + hintCooldown;
                }
                break;
            }
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            foreach (var part in dungeonParts)
            {
                if (Vector2.Distance(playerPosition, part.MonumentPosition) < 1.5f)
                {
                    CompletePart(part);
                    audioManager.PlaySFX(audioManager.placeColorCore);
                    break;
                }
            }
        }


        if (countdown.CountdownFinished)
        {
            AlertText.Instance.ShowAlert("HOPE YOU MAKE IT NEXT TIME!");
            player.isDead = true;
            SaveSystem.SavePlayer(player);
            audioManager.StopTickingSound();
            isTeleportedToFinalLevel = false;
        }

        if (Input.GetKeyDown(KeyCode.G) && !isTeleportedToFinalLevel && player.HasAllColorCores())
        {
            isTeleportedToFinalLevel = true;
            PlacePlayerRandomly(player);
            countdown.remainingTime = 300;
            countdown.StartCountdown = true;
            countdown.isCountdownForFinalLevel = true;
            AlertText.Instance.ShowAlert("PLACE ALL COLOR CORES TO THEIR RIGHT PLACE! CAREFUL! YOUR TIME IS LIMITED!", 5f);
        }
        if (PlayerPrefs.GetInt("DevTools", 0) == 1)
        {
            if (Input.GetKeyDown(KeyCode.U))
            {
                player.transform.position = new Vector3(dungeonParts[0].MonumentPosition.x + 1, dungeonParts[0].MonumentPosition.y + 1, 0);
            }
            if (Input.GetKeyDown(KeyCode.I))
            {
                player.transform.position = new Vector3(dungeonParts[1].MonumentPosition.x + 1, dungeonParts[1].MonumentPosition.y + 1, 0);
            }
            if (Input.GetKeyDown(KeyCode.O))
            {
                player.transform.position = new Vector3(dungeonParts[2].MonumentPosition.x + 1, dungeonParts[2].MonumentPosition.y + 1, 0);
            }
            if (Input.GetKeyDown(KeyCode.P))
            {
                player.transform.position = new Vector3(dungeonParts[3].MonumentPosition.x + 1, dungeonParts[3].MonumentPosition.y + 1, 0);
            }
        }
        if (AllCoresPlaced() && isTeleportedToFinalLevel)
        {
            FadeManager.Instance.FadeToScene("GameEnding");
        }

    }

    private bool AllCoresPlaced()
    {
        foreach (var part in dungeonParts)
        {
            if (part.ColorCore == null || !part.ColorCore.isPlaced)
            {
                return false;
            }
        }
        return true;
    }


    public void GenerateFinalLevel()
    {
        ClearLevel();

        mazeBounds = new BoundsInt(startCoordinates.x, startCoordinates.y, 0, levelSize, levelSize, 1);

        int halfSize = levelSize / 2;

        dungeonParts.Add(new FinalDungeonPart(new BoundsInt(mazeBounds.min.x, mazeBounds.min.y, 0, halfSize, halfSize, 1), DungeonColor.Pink));
        dungeonParts.Add(new FinalDungeonPart(new BoundsInt(mazeBounds.min.x + halfSize, mazeBounds.min.y, 0, halfSize, halfSize, 1), DungeonColor.Blue));
        dungeonParts.Add(new FinalDungeonPart(new BoundsInt(mazeBounds.min.x, mazeBounds.min.y + halfSize, 0, halfSize, halfSize, 1), DungeonColor.Green));
        dungeonParts.Add(new FinalDungeonPart(new BoundsInt(mazeBounds.min.x + halfSize, mazeBounds.min.y + halfSize, 0, halfSize, halfSize, 1), DungeonColor.Purple));

        Vector2Int startPosition = new Vector2Int((int)mazeBounds.center.x, (int)mazeBounds.center.y);
        GenerateMaze(startPosition);

        DrawLevel();
        DrawBoundaryWalls();
        PlaceMonuments();

        Debug.Log("Final level with maze and sections generated.");
    }

    private void GenerateMaze(Vector2Int start)
    {
        Stack<Vector2Int> stack = new Stack<Vector2Int>();
        HashSet<Vector2Int> visited = new HashSet<Vector2Int>();

        stack.Push(start);
        visited.Add(start);

        while (stack.Count > 0)
        {
            Vector2Int current = stack.Pop();
            mazeFloor.Add(current);

            List<Vector2Int> neighbors = GetUnvisitedNeighbors(current, visited);
            if (neighbors.Count > 0)
            {
                stack.Push(current);

                Vector2Int chosenNeighbor = neighbors[Random.Range(0, neighbors.Count)];
                visited.Add(chosenNeighbor);

                // Pøidání cesty mezi buòkami
                Vector2Int path = (current + chosenNeighbor) / 2;
                mazeFloor.Add(path);

                // Pøidání sousedních zdí okolo cesty
                AddSurroundingWalls(current);
                AddSurroundingWalls(path);
                AddSurroundingWalls(chosenNeighbor);

                stack.Push(chosenNeighbor);
            }
        }
    }

    private void PlaceMonuments()
    {
        foreach (var part in dungeonParts)
        {
            Vector2Int monumentPosition = PlaceMonumentInBounds(part.Bounds);
            var monument = Instantiate(monumentPrefab, new Vector3(monumentPosition.x + 0.5f, monumentPosition.y +0.5f, 0), Quaternion.identity);
            ColorCore colorCore = Instantiate(GetColorCore(part.Color), new Vector3(monumentPosition.x + 0.52f, monumentPosition.y + 0.85f, 0), Quaternion.identity);
            part.ColorCore = colorCore;
            part.ColorCore.gameObject.SetActive(false);
            allItems.Add(monument);
            part.SetMonument(monumentPosition, monument);
        }
    }

    private Vector2Int PlaceMonumentInBounds(BoundsInt bounds)
    {
        Vector2Int center = new Vector2Int(bounds.xMin + bounds.size.x / 2, bounds.yMin + bounds.size.y / 2);

        for (int x = center.x - 1; x <= center.x + 1; x++)
        {
            for (int y = center.y - 1; y <= center.y + 1; y++)
            {
                Vector2Int position = new Vector2Int(x, y);

                mazeFloor.Add(position);
                mapCreator.DrawTile(floorTile, new Vector2Int(x, y));
                colliderMap.SetTile(new Vector3Int(x, y, 0), null);
            }
        }

        // Pøidat zdi kolem 3x3 místnosti
        for (int x = center.x - 2; x <= center.x + 2; x++)
        {
            for (int y = center.y - 2; y <= center.y + 2; y++)
            {
                // Ignorovat pozice podlahy (3x3 místnost)
                if (x >= center.x - 1 && x <= center.x + 1 && y >= center.y - 1 && y <= center.y + 1)
                    continue;

                if ((x == center.x && y == center.y - 2) ||
                    (x == center.x && y == center.y + 2) ||
                    (x == center.x - 2 && y == center.y) ||
                    (x == center.x + 2 && y == center.y))  
                    continue;

                Vector2Int wallPosition = new Vector2Int(x, y);

                // Nastavit zdi
                mapCreator.DrawTile(wallTile, wallPosition);
                mazeFloor.Remove(wallPosition);
                AddCollider(wallPosition);
            }
        }

        // Pøidat vchody a zahrnout je do mazeFloor
        Vector2Int[] entrancePositions = new Vector2Int[]
        {
        new Vector2Int(center.x, center.y - 2),
        new Vector2Int(center.x - 2, center.y),
        new Vector2Int(center.x + 2, center.y),
        new Vector2Int(center.x, center.y + 2),
        new Vector2Int(center.x, center.y - 3),
        new Vector2Int(center.x - 3, center.y),
        new Vector2Int(center.x + 3, center.y),
        new Vector2Int(center.x, center.y + 3) 
        };

        Vector2Int[] cornerPositions = new Vector2Int[]
        {
            new Vector2Int(center.x - 1, center.y - 1),
            new Vector2Int(center.x - 1, center.y + 1),
            new Vector2Int(center.x + 1, center.y - 1),
            new Vector2Int(center.x + 1, center.y + 1)
        };

        foreach (Vector2Int corner in cornerPositions)
        {
            Item firePedestal = Instantiate(firePedestalPrefab, new Vector3(corner.x + 0.5f, corner.y + 0.75f, 0), Quaternion.identity);
        }

        foreach (Vector2Int entrance in entrancePositions)
        {
            mazeFloor.Add(entrance);
            mapCreator.DrawTile(floorTile, entrance);
            colliderMap.SetTile(new Vector3Int(entrance.x, entrance.y, 0), null); 
        }

        return center;
    }

    private UnityEngine.Tilemaps.Tile GetColoredFloorTile(DungeonColor color)
    {
        return color switch
        {
            DungeonColor.Pink => pinkFloorTile,
            DungeonColor.Blue => blueFloorTile,
            DungeonColor.Green => greenFloorTile,
            DungeonColor.Purple => purpleFloorTile,
            _ => floorTile
        };
    }

    private ColorCore GetColorCore(DungeonColor color)
    {
        return color switch
        {
            DungeonColor.Pink => pinkColorCore,
            DungeonColor.Blue => blueColorCore,
            DungeonColor.Green => greenColorCore,
            DungeonColor.Purple => purpleColorCore,
        };
    }

    private IEnumerable<Vector2Int> GetPositionsInBounds(BoundsInt bounds)
    {
        for (int x = bounds.xMin; x < bounds.xMax; x++)
        {
            for (int y = bounds.yMin; y < bounds.yMax; y++)
            {
                yield return new Vector2Int(x, y);
            }
        }
    }

    private bool IsMazeFullyConnected()
    {
        HashSet<Vector2Int> visited = new HashSet<Vector2Int>();
        Queue<Vector2Int> queue = new Queue<Vector2Int>();
        Vector2Int start = new Vector2Int((int)mazeBounds.center.x, (int)mazeBounds.center.y);

        queue.Enqueue(start);
        visited.Add(start);

        while (queue.Count > 0)
        {
            Vector2Int current = queue.Dequeue();

            foreach (Vector2Int direction in Directions.eightDirectionsList)
            {
                Vector2Int neighbor = current + direction;
                if (mazeFloor.Contains(neighbor) && !visited.Contains(neighbor))
                {
                    visited.Add(neighbor);
                    queue.Enqueue(neighbor);
                }
            }
        }

        return visited.Count == mazeFloor.Count;
    }

    private void AddSurroundingWalls(Vector2Int position)
    {
        foreach (Vector2Int direction in Directions.eightDirectionsList)
        {
            Vector2Int wallPosition = position + direction;
            if (!mazeFloor.Contains(wallPosition))
            {
                mazeFloor.Add(wallPosition);
            }
        }

    }



    private List<Vector2Int> GetUnvisitedNeighbors(Vector2Int current, HashSet<Vector2Int> visited)
    {
        List<Vector2Int> directions = new List<Vector2Int>
    {
        new Vector2Int(0, 4),
        new Vector2Int(4, 0),
        new Vector2Int(0, -4),
        new Vector2Int(-4, 0)
    };

        List<Vector2Int> neighbors = new List<Vector2Int>();

        foreach (Vector2Int direction in directions)
        {
            Vector2Int neighbor = current + direction;
            if (!visited.Contains(neighbor) && mazeBounds.Contains(new Vector3Int(neighbor.x, neighbor.y, 0)))
            {
                neighbors.Add(neighbor);
            }
        }

        return neighbors;
    }


    private void DrawLevel()
    {
        for (int x = mazeBounds.xMin; x < mazeBounds.xMax; x++)
        {
            for (int y = mazeBounds.yMin; y < mazeBounds.yMax; y++)
            {
                Vector2Int position = new Vector2Int(x, y);
                if (mazeFloor.Contains(position))
                {
                    mapCreator.DrawTile(floorTile, position);
                }
                else
                {
                    mapCreator.DrawTile(wallTile, position);
                    AddCollider(position);
                }
            }
        }
    }

    private void DrawBoundaryWalls()
    {
        for (int x = mazeBounds.xMin; x <= mazeBounds.xMax; x++)
        {
            mapCreator.DrawTile(wallTile, new Vector2Int(x, mazeBounds.yMin));
            mapCreator.DrawTile(wallTile, new Vector2Int(x, mazeBounds.yMax));
        }

        for (int y = mazeBounds.yMin; y <= mazeBounds.yMax; y++)
        {
            mapCreator.DrawTile(wallTile, new Vector2Int(mazeBounds.xMin, y));
            mapCreator.DrawTile(wallTile, new Vector2Int(mazeBounds.xMax, y));
        }
    }

    private void CompletePart(FinalDungeonPart part)
    {
        part.ColorCore.gameObject.SetActive(true);
        part.ColorCore.isPlaced = true;

        player.DisableLightForDuration(8);
        // Spustí postupné zvìtšování svìtla
        part.ColorCore.GradualLightIncrease(10f);

        // Spustí postupné nahrazování podlahy
        StartCoroutine(GradualFloorReplacement(part));
    }

    private IEnumerator GradualFloorReplacement(FinalDungeonPart part)
    {
        Vector2Int center = new Vector2Int(part.Bounds.xMin + part.Bounds.size.x / 2, part.Bounds.yMin + part.Bounds.size.y / 2);

        List<Vector2Int> positions = new List<Vector2Int>(GetPositionsInBounds(part.Bounds));

        positions.Sort((a, b) => Vector2Int.Distance(a, center).CompareTo(Vector2Int.Distance(b, center)));

        float duration = 0.1f;
        foreach (var position in positions)
        {
            if (mazeFloor.Contains(position))
            {
                mapCreator.DrawTile(GetColoredFloorTile(part.Color), position);

                yield return new WaitForSeconds(duration);
                duration -= 0.001f;
            }
        }

        Debug.Log($"Completed gradual floor replacement for part {part.Color}.");
    }


    public void PlacePlayerRandomly(Player player)
    {
        if (player == null)
        {
            Debug.LogWarning("Player object is not assigned.");
            return;
        }

        List<Vector2Int> floorPositions = new List<Vector2Int>(mazeFloor);
        Vector2Int randomPosition = floorPositions[Random.Range(0, floorPositions.Count)];

        player.transform.position = new Vector3(randomPosition.x + 0.5f, randomPosition.y + 0.5f, 0);
        Debug.Log("Player placed at: " + randomPosition);
        TurnOffGlobalLight();
    }

    public void AddCollider(Vector2Int position)
    {
        Vector3Int tilePosition = this.colliderMap.WorldToCell((Vector3Int)position);
        this.colliderMap.SetTile(tilePosition, emptyTile);
    }

    private void ClearLevel()
    {
        colliderMap.ClearAllTiles();
        mazeFloor.Clear();
        dungeonParts.Clear();
        foreach (Item item in allItems)
        {
            if (item != null)
            {
                DestroyImmediate(item.gameObject);
            }
        }
    }

    public void TurnOffGlobalLight()
    {
        if (globalLight != null)
        {
            globalLight.intensity = 0f;
        }
        else
        {
            Debug.LogWarning("Global Light is not assigned!");
        }
    }

}