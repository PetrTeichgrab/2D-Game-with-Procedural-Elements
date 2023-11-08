using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public static class WallGenerator
{
    public static void CreateAndDrawWalls(HashSet<Vector2Int> floor, MapCreator mapCreator, Color color)
    {
        HashSet<Vector2Int> cardinalWalls = FindWallsInCardinalDirections(floor);

        HashSet<Vector2Int> diagonalWalls = FindWallsInDiagonalDirections(floor, cardinalWalls);

        mapCreator.DrawCardinalWalls(cardinalWalls, color);

        mapCreator.DrawDiagonalWalls(diagonalWalls, color);
    }

    private static HashSet<Vector2Int> FindWallsInCardinalDirections(HashSet<Vector2Int> floor)
    {
        HashSet<Vector2Int> walls = new HashSet<Vector2Int>();

        foreach (Vector2Int position in floor) { 

            foreach (Vector2Int direction in Directions.DirectionsDic.Values)
            {
                if(!floor.Contains(position + direction))
                {
                    walls.Add(position + direction);
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
            foreach (Vector2Int direction in Directions.DiagonalDirectionsDic.Values)
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
