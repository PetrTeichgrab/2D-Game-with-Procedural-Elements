using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class RandomWalkAlgorithms
{
    //Random walk algorithm
    public static HashSet<Vector2Int> RandomWalk(RandomWalkParameters parameters, Vector2Int startingPosition)
    {
        var currentWalkPosition = startingPosition;

        int length = UnityEngine.Random.Range(parameters.minLength, parameters.maxLength);

        HashSet<Vector2Int> floorPosition = new HashSet<Vector2Int>();

        for (int i = 0; i < parameters.iterations; i++)
        {
            currentWalkPosition = startingPosition;
            floorPosition.Add(currentWalkPosition);
            for (int j = 0; j < length; j++)
            {
                Vector2Int newPosition = currentWalkPosition + Directions.GetRandomCardinalDirection();
                floorPosition.Add(newPosition);
                currentWalkPosition = newPosition;
            }
            //if (parameters.startRandom)
            //{
            //    currentWalkPosition = floorPosition.ElementAt(Random.Range(0, floorPosition.Count));
            //}
        }
        return floorPosition;
    }

    //Method that creates corridor
    public static List<Vector2Int> RandomOneDirectionWalk(Vector2Int startingPosition, int length)
    {
        List<Vector2Int> corridor = new List<Vector2Int>();
        Vector2Int direction = Directions.GetRandomCardinalDirection();
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
