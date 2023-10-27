using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using UnityEngine;

public static class FloorGenerator
{
    
    public static void CreateRooms(RandomWalkParameters parameters, HashSet<Vector2Int> rooms)
    {
        HashSet<Vector2Int> roomsTmp = new HashSet<Vector2Int>();

        foreach (var room in rooms)
        {
            var roomFloor = RandomWalkAlgorithms.RandomWalk(parameters, room);
            roomsTmp.UnionWith(roomFloor);
        }
        rooms.UnionWith(roomsTmp);
    }

    //TODO: optimalization
    //Algorithm for filling holes
    public static void FillHoles(HashSet<Vector2Int> floor)
    {
        HashSet<Vector2Int> holes = new HashSet<Vector2Int>();

        foreach (var position in floor)
        {
            Vector2Int hole = Vector2Int.zero;
            foreach (Vector2Int direction in Directions.DirectionsDic.Values)
            {
                hole = position + direction;

                int counter = 0;

                if (!floor.Contains(hole))
                {
                    foreach (var holeDireciton in Directions.DirectionsDic.Values)
                    {
                        if (floor.Contains(holeDireciton))
                        {
                            counter++;
                        }
                    }
                }
                if (counter >= 3)
                {
                    holes.Add(hole);
                }

            }
        }
        floor.UnionWith(holes);
    }

    public static void CreateCorridors(HashSet<Vector2Int> floor, HashSet<Vector2Int> rooms
        ,Vector2Int startPosition, int amountOfCorridors, int corridorLength)
    {
        var currentCorridorEndPosition = startPosition;

        rooms.Add(currentCorridorEndPosition);

        for (int i = 0; i < amountOfCorridors; i++)
        {
            var corridor = RandomWalkAlgorithms.RandomOneDirectionWalk(currentCorridorEndPosition, corridorLength);
            currentCorridorEndPosition = corridor[corridor.Count - 1];
            rooms.Add(currentCorridorEndPosition);
            floor.UnionWith(corridor);
        }
    }
}
