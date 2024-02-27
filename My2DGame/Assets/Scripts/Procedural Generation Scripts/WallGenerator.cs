using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public static class WallGenerator
{
    public static void CreateAndDrawWalls(Dungeon dungeon, MapCreator mapCreator)
    {
        HashSet<Vector2Int> cardinalWalls = FindWallsInCardinalDirections(dungeon.Floor.FloorList, dungeon.Floor.AnotherDungeonsEntrances);

        HashSet<Vector2Int> diagonalWalls = FindWallsInDiagonalDirections(dungeon.Floor.FloorList, cardinalWalls, dungeon.Floor.AnotherDungeonsEntrances);

        mapCreator.DrawCardinalWalls(cardinalWalls, dungeon.Color);

        mapCreator.DrawDiagonalWalls(diagonalWalls, dungeon.Color);
    }

    private static HashSet<Vector2Int> FindWallsInCardinalDirections(HashSet<Vector2Int> floor, List<Vector2Int> anotherDungeonsEntrances)
    {
        HashSet<Vector2Int> walls = new HashSet<Vector2Int>();

        foreach (Vector2Int position in floor) { 

            foreach (Vector2Int direction in Directions.CardinalDirectionsDic.Values)
            {
                if((!floor.Contains(position + direction)) && !anotherDungeonsEntrances.Contains(position + direction))
                {
                    walls.Add(position + direction);
                }
            }
        }
        return walls;
    }

    private static HashSet<Vector2Int> FindWallsInDiagonalDirections(HashSet<Vector2Int> floor, HashSet<Vector2Int> cardinalWalls, List<Vector2Int> anotherDungeonsEntrances)
    {
        HashSet<Vector2Int> walls = new HashSet<Vector2Int>();
        bool isEntrance = false;

        foreach (Vector2Int position in floor)
        {
            foreach (Vector2Int direction in Directions.CardinalDirectionsDic.Values)
            {
                if (anotherDungeonsEntrances.Contains(position + direction))
                {
                    isEntrance = true;
                    break;
                }
            }
            foreach (Vector2Int direction in Directions.DiagonalDirectionsDic.Values)
            {
                if (!floor.Contains(position + direction) && !cardinalWalls.Contains(position + direction) && !isEntrance)
                {
                    walls.Add(position + direction);
                }
            }
            isEntrance = false;
        }
        return walls;
    }
}
