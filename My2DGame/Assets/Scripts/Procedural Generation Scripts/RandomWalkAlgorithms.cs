using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class RandomWalkAlgorithms
{
    public static HashSet<Vector2Int> RandomWalk(RandomWalkParameters parameters, Vector2Int startingPosition)
    {
        var currentWalkPosition = startingPosition;

        HashSet<Vector2Int> floorPosition = new HashSet<Vector2Int>();

        for (int i = 0; i < parameters.iterations; i++)
        {
            floorPosition.Add(currentWalkPosition);
            for (int j = 0; j < parameters.length; j++)
            {
                Vector2Int newPosition = currentWalkPosition + Directions.GetRandomDirection();
                floorPosition.Add(newPosition);
                currentWalkPosition = newPosition;
            }
            if (parameters.startRandom)
            {
                currentWalkPosition = floorPosition.ElementAt(Random.Range(0, floorPosition.Count));
            }
        }
        return floorPosition;
    }
    public static List<Vector2Int> RandomOneDirectionWalk(Vector2Int startingPosition, int length)
    {
        List<Vector2Int> corridor = new List<Vector2Int>();
        Vector2Int direction = Directions.GetRandomDirection();
        Vector2Int currentPosition = startingPosition;
        corridor.Add(currentPosition);

        //TODO: možnost udìlat chodbu s nastavitelnou šíøkou
        for (int i = 0; i < length; i++)
        {
            currentPosition += direction;
            corridor.Add(currentPosition);
        }
        return corridor;
    }
}
