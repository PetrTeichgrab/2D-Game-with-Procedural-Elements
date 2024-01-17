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

    public static HashSet<Vector2Int> ConnectRooms(List<Vector2Int> roomCenters)
    {
        HashSet<Vector2Int> corridors = new HashSet<Vector2Int>();
        var currentRoomCenter = roomCenters[UnityEngine.Random.Range(0, roomCenters.Count)];
        roomCenters.Remove(currentRoomCenter);

        while (roomCenters.Count > 0)
        {
            Vector2Int closest = FindClosestPoint(currentRoomCenter, roomCenters);
            roomCenters.Remove(closest);
            HashSet<Vector2Int> newCorridor = CreateCorridor(currentRoomCenter, closest);
            currentRoomCenter = closest;
            corridors.UnionWith(newCorridor);
        }
        return corridors;
    }


    public static HashSet<Vector2Int> CreateCorridor(Vector2Int currentRoomCenter, Vector2Int closest)
    {
        HashSet<Vector2Int> corridor = new HashSet<Vector2Int>();
        var position = currentRoomCenter;
        corridor.Add(position);
        while (position.y != closest.y)
        {
            if (closest.y > position.y)
            {
                position += Vector2Int.up;
            }
            else if (closest.y < position.y)
            {
                position += Vector2Int.down;
            }
            corridor.Add((Vector2Int)position);
        }
        while (position.x != closest.x)
        {
            if (closest.x > position.x)
            {
                position += Vector2Int.right;
            }
            else if (closest.x < position.x)
            {
                position += Vector2Int.left;
            }
            corridor.Add((Vector2Int)position);
        }
        return corridor;
    }


    public static HashSet<Vector2Int> CreateRandomRooms(List<BoundsInt> roomList, RandomWalkParameters randomWalkParameters, int offset)
    {
        HashSet<Vector2Int> floor = new HashSet<Vector2Int>();
        for (int i = 0; i < roomList.Count; i++)
        {
            var roomBounds = roomList[i];
            var roomCenter = new Vector2Int(Mathf.RoundToInt(roomBounds.center.x), Mathf.RoundToInt(roomBounds.center.y));
            var roomFloor = RandomWalkAlgorithms.RandomWalk(randomWalkParameters, roomCenter);
            foreach (var pos in roomFloor)
            {
                if (pos.x >= (roomBounds.xMin + offset) && pos.x <= (roomBounds.xMax - offset) &&
                    pos.y <= (roomBounds.yMax - offset) && pos.y >= (roomBounds.yMin + offset))
                {
                    floor.Add(pos);
                }
            }
        }
        return floor;
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

    public static Vector2Int FindClosestPoint(Vector2Int startingPoint, List<Vector2Int> listOfPoints)
    {
        Vector2Int closest = Vector2Int.zero;
        float distance = float.MaxValue;
        foreach (var position in listOfPoints)
        {
            float currentDistance = Vector2.Distance(position, startingPoint);
            if (currentDistance < distance)
            {
                distance = currentDistance;
                closest = position;
            }
        }
        return closest;
    }
}
