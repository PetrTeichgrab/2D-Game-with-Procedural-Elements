using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public static class WallGenerator
{
    public static void CreateAndDrawWalls(Dungeon dungeon, MapCreator mapCreator)
    {
        HashSet<Vector2Int> cardinalWalls = FindWallsInCardinalDirections(dungeon.Floor.FloorList, dungeon.Floor.Edges);

        HashSet<Vector2Int> diagonalWalls = FindWallsInDiagonalDirections(dungeon.Floor.FloorList, cardinalWalls);

        mapCreator.DrawCardinalWalls(cardinalWalls, dungeon.Floor.FloorList, dungeon.Color);

        mapCreator.DrawDiagonalWalls(diagonalWalls, dungeon.Floor.FloorList, dungeon.Color);
    }

    private static HashSet<Vector2Int> FindWallsInCardinalDirections(HashSet<Vector2Int> floor, HashSet<Vector2Int> edges)
    {
        HashSet<Vector2Int> walls = new HashSet<Vector2Int>();

        foreach (Vector2Int position in floor) { 

            foreach (Vector2Int direction in Directions.cardinalDirectionsList)
            {
                if((!floor.Contains(position + direction)))
                {
                    walls.Add(position + direction);
                    edges.Add(position);
                }
            }
        }
        return walls;
    }

    private static HashSet<Vector2Int> FindWallsInDiagonalDirections(HashSet<Vector2Int> floor, HashSet<Vector2Int> cardinalWalls)
    {
        HashSet<Vector2Int> walls = new HashSet<Vector2Int>();

        foreach (Vector2Int position in floor)
        {
            foreach (Vector2Int direction in Directions.diagonalDirectionsList)
            {
                if (!floor.Contains(position + direction) && !cardinalWalls.Contains(position + direction))
                {
                    walls.Add(position + direction);
                }
            }
        }
        return walls;
    }
}
