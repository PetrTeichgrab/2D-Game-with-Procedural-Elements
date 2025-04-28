using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using UnityEngine;

public static class FloorGenerator
{
    public static HashSet<Vector2Int> ConnectRooms(List<Vector2Int> roomCenters)
    {
        HashSet<Vector2Int> corridors = new HashSet<Vector2Int>();

        if (roomCenters.Count == 0) return corridors;

        var currentRoomCenter = roomCenters[UnityEngine.Random.Range(0, roomCenters.Count)];
        roomCenters.Remove(currentRoomCenter);

        while (roomCenters.Count > 0)
        {
            Vector2Int closest = FindClosestPoint(currentRoomCenter, roomCenters);
            roomCenters.Remove(closest);

            HashSet<Vector2Int> newCorridor = CreateCurvedCorridor(currentRoomCenter, closest);
            currentRoomCenter = closest;
            corridors.UnionWith(newCorridor);
        }

        return corridors;
    }

    public static HashSet<Vector2Int> CreateCurvedCorridor(Vector2Int start, Vector2Int end)
    {
        HashSet<Vector2Int> corridor = new HashSet<Vector2Int>();

        // Pøidáme mírnì náhodný bod mezi start a end pro zakøivení
        Vector2Int midPoint = new Vector2Int((start.x + end.x) / 2, (start.y + end.y) / 2);
        midPoint += new Vector2Int(UnityEngine.Random.Range(-2, 2), UnityEngine.Random.Range(-2, 2));

        List<Vector2Int> firstHalf = GeneratePath(start, midPoint);
        List<Vector2Int> secondHalf = GeneratePath(midPoint, end);

        corridor.UnionWith(firstHalf);
        corridor.UnionWith(secondHalf);

        return corridor;
    }

    private static List<Vector2Int> GeneratePath(Vector2Int start, Vector2Int end)
    {
        List<Vector2Int> path = new List<Vector2Int>();
        Vector2Int current = start;

        while (current != end)
        {
            path.Add(current);

            if (UnityEngine.Random.value > 0.5f)
            {
                if (current.x < end.x) current.x++;
                else if (current.x > end.x) current.x--;
            }
            else
            {
                if (current.y < end.y) current.y++;
                else if (current.y > end.y) current.y--;
            }
        }

        path.Add(end);
        return path;
    }



    public static HashSet<Vector2Int> CreateCorridor(Vector2Int currentRoomCenter, Vector2Int closest)
    {
        HashSet<Vector2Int> corridor = new HashSet<Vector2Int>();
        HashSet<Vector2Int> corridorFloorTmp = new HashSet<Vector2Int>();
        var position = currentRoomCenter;
        corridor.Add(position);
        while (position.y != closest.y)
        {
            if (closest.y > position.y)
            {
                position += Vector2Int.up;
                corridor.Add(position);
                corridor.Add(position + Vector2Int.right);
                corridor.Add(position + Vector2Int.left);
            }
            else if (closest.y < position.y)
            {
                position += Vector2Int.down;

                corridor.Add(position + Vector2Int.right);
                corridor.Add(position + Vector2Int.left);
                corridor.Add(position);
            }
        }
        while (position.x != closest.x)
        {
            if (closest.x > position.x)
            {
                position += Vector2Int.right;
                corridor.Add(position + Vector2Int.right);
                corridor.Add(position + Vector2Int.up);
                corridor.Add(position);
            }
            else if (closest.x < position.x)
            {
                position += Vector2Int.left;

                corridor.Add(position + Vector2Int.left);
                corridor.Add(position + Vector2Int.up);
                corridor.Add(position);
            }
        }
        return corridor;
    }

    public static HashSet<Vector2Int> CreateRandomRooms(List<Room> roomList, List<BoundsInt> roomBoundsList, RandomWalkParameters randomWalkParameters, int offset)
    {
        HashSet<Vector2Int> floor = new HashSet<Vector2Int>();
        foreach (var roomBounds in roomBoundsList)
        {
            var roomCenter = new Vector2Int(Mathf.RoundToInt(roomBounds.center.x), Mathf.RoundToInt(roomBounds.center.y));
            var roomFloor = RandomWalkAlgorithms.RandomWalk(randomWalkParameters, roomCenter);
            HashSet<Vector2Int> actualRoomFloor = new HashSet<Vector2Int>();
            foreach (var pos in roomFloor)
            {
                if (pos.x >= (roomBounds.xMin + offset) && pos.x <= (roomBounds.xMax - offset) &&
                    pos.y <= (roomBounds.yMax - offset) && pos.y >= (roomBounds.yMin + offset))
                {
                    floor.Add(pos);
                    actualRoomFloor.Add(pos);
                }
            }
            roomList.Add(new Room(actualRoomFloor, roomCenter));
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
            int counter = 0;

            foreach (Vector2Int direction in Directions.AllDirectionsDic.Values)
            {
                hole = position + direction;

                if (!floor.Contains(hole))
                {
                    foreach (var holeDireciton in Directions.AllDirectionsDic.Values)
                    {
                        if (floor.Contains(holeDireciton))
                        {
                            counter++;
                        }
                    }
                }
                if (counter >= 4)
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

    public static Vector2Int FindNearestPoint(Vector2Int origin, List<Vector2Int> points)
    {
        Vector2Int nearestPoint = Vector2Int.zero;
        float shortestDistance = float.MaxValue;

        foreach (Vector2Int point in points)
        {
            // Spoèítáme Manhattanovu vzdálenost
            float distance = Mathf.Abs(point.x - origin.x) + Mathf.Abs(point.y - origin.y);

            if (distance < shortestDistance)
            {
                shortestDistance = distance;
                nearestPoint = point;
            }
        }

        return nearestPoint;
    }
}
