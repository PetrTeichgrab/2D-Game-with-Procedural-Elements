using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.WSA;

public class FinalLevelGenerator : MonoBehaviour
{
    [SerializeField]
    private int levelSize = 99;

    [SerializeField]
    private Tilemap tilemap;

    [SerializeField]
    private UnityEngine.Tilemaps.Tile completedFloorTile, emptyTile, floorTile, wallTile;

    [SerializeField]
    private Tilemap colliderMap;

    [SerializeField]
    private MapCreator mapCreator;

    [SerializeField]
    private Vector2Int startCoordinates = new Vector2Int(500, 500);

    private BoundsInt mazeBounds;
    private BoundsInt part1Bounds;
    private BoundsInt part2Bounds;
    private BoundsInt part3Bounds;
    private BoundsInt part4Bounds;

    private HashSet<Vector2Int> mazeFloor = new HashSet<Vector2Int>();

    public void GenerateFinalLevel()
    {
        ClearLevel();

        mazeBounds = new BoundsInt(startCoordinates.x, startCoordinates.y, 0, levelSize, levelSize, 1);

        int halfSize = levelSize / 2;
        part1Bounds = new BoundsInt(mazeBounds.min.x, mazeBounds.min.y, 0, halfSize, halfSize, 1);
        part2Bounds = new BoundsInt(mazeBounds.min.x + halfSize, mazeBounds.min.y, 0, halfSize, halfSize, 1);
        part3Bounds = new BoundsInt(mazeBounds.min.x, mazeBounds.min.y + halfSize, 0, halfSize, halfSize, 1);
        part4Bounds = new BoundsInt(mazeBounds.min.x + halfSize, mazeBounds.min.y + halfSize, 0, halfSize, halfSize, 1);

        Vector2Int startPosition = new Vector2Int((int)mazeBounds.center.x, (int)mazeBounds.center.y);
        GenerateMaze(startPosition);

        DrawLevel();

        DrawBoundaryWalls();

        Debug.Log("Final level with maze and sections generated using BoundsInt.");
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
                mazeFloor.Add(chosenNeighbor);
                visited.Add(chosenNeighbor);

                // Pøidání cesty mezi buòkami
                Vector2Int path = (current + chosenNeighbor) / 2;
                mazeFloor.Add(path);

                stack.Push(chosenNeighbor);
            }
        }
    }

    private List<Vector2Int> GetUnvisitedNeighbors(Vector2Int current, HashSet<Vector2Int> visited)
    {
        List<Vector2Int> directions = new List<Vector2Int>
        {
            new Vector2Int(0, 2),
            new Vector2Int(2, 0),
            new Vector2Int(0, -2),
            new Vector2Int(-2, 0)
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
                else {
                    mapCreator.DrawTile(wallTile,position);
                    AddCollider(position);
                }
            }
        }
    }

    private void DrawBoundaryWalls()
    {
        for (int x = mazeBounds.xMin; x <= mazeBounds.xMax; x++)
        {
            tilemap.SetTile(new Vector3Int(x, mazeBounds.yMin, 0), wallTile);
            tilemap.SetTile(new Vector3Int(x, mazeBounds.yMax - 1, 0), wallTile);
        }

        for (int y = mazeBounds.yMin; y <= mazeBounds.yMax; y++)
        {
            tilemap.SetTile(new Vector3Int(mazeBounds.xMin, y, 0), wallTile);
            tilemap.SetTile(new Vector3Int(mazeBounds.xMax - 1, y, 0), wallTile);
        }
    }

    public void CompletePart(int partNumber)
    {
        BoundsInt partBounds = partNumber switch
        {
            1 => part1Bounds,
            2 => part2Bounds,
            3 => part3Bounds,
            4 => part4Bounds,
            _ => default
        };

        if (partBounds == default) return;

        for (int x = partBounds.xMin; x < partBounds.xMax; x++)
        {
            for (int y = partBounds.yMin; y < partBounds.yMax; y++)
            {
                Vector2Int position = new Vector2Int(x, y);
                if (mazeFloor.Contains(position))
                {
                    tilemap.SetTile(new Vector3Int(x, y, 0), completedFloorTile);
                }
            }
        }

        Debug.Log($"Part {partNumber} completed and floor color updated.");
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
    }

    public void AddCollider(Vector2Int position)
    {
        Vector3Int tilePosition = this.colliderMap.WorldToCell((Vector3Int)position);
        this.colliderMap.SetTile(tilePosition, emptyTile);
    }

    private void ClearLevel()
    {
        tilemap.ClearAllTiles();
        mazeFloor.Clear();
    }
}
