using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

public static class ProceduralGenerationAlgorithms
{
    public static HashSet<BoundsInt> BSP(BoundsInt spaceToSplit, int minWidth, int minHeight)
    {
        Queue<BoundsInt> toBeSplitObjects = new Queue<BoundsInt>();
        HashSet<BoundsInt> splittedObjects = new HashSet<BoundsInt>();
        toBeSplitObjects.Enqueue(spaceToSplit);

        while (toBeSplitObjects.Count > 0)
        {
            var toBeSplitObject = toBeSplitObjects.Dequeue();

            if (toBeSplitObject.size.x < minWidth * 2 && toBeSplitObject.size.y < minHeight * 2)
            {
                splittedObjects.Add(toBeSplitObject);
                continue;
            }

            bool splitHorizontally;

            if (toBeSplitObject.size.x >= minWidth * 2 && toBeSplitObject.size.y >= minHeight * 2)
            {
                splitHorizontally = UnityEngine.Random.value > 0.5f;
            }
            else
            {
                splitHorizontally = toBeSplitObject.size.y >= minHeight * 2;
            }

            if (splitHorizontally && toBeSplitObject.size.y >= minHeight * 2)
            {
                SplitHorizontally(minHeight, toBeSplitObjects, toBeSplitObject);
            }
            else if (!splitHorizontally && toBeSplitObject.size.x >= minWidth * 2)
            {
                SplitVertically(minWidth, toBeSplitObjects, toBeSplitObject);
            }
            else
            {
                splittedObjects.Add(toBeSplitObject);
            }
        }

        return splittedObjects;
    }


    public static HashSet<Vector2Int> PerlinNoise(Underground underground, int height, int width, float smoothness, float modifier, int startX = 0)
    {
        int perlinHeight = 0;
        int seed = UnityEngine.Random.Range(-100000, 100000);
        HashSet<Vector2Int> undergroundMap = new HashSet<Vector2Int>();
        HashSet<Vector2Int> boundaries = new HashSet<Vector2Int>();

        for (int x = startX; x < width + startX; x++)
        {
            float perlinValue = Mathf.PerlinNoise((x / smoothness) + seed, seed);
            perlinHeight = Mathf.RoundToInt(perlinValue * (height / 2)) + (height / 2);

            // Spodní hrana
            for (int i = -5; i <= 0; i++)
            {
                boundaries.Add(new Vector2Int(x, i));
            }

            for (int y = 0; y < perlinHeight; y++)
            {
                float caveValue = Mathf.PerlinNoise((x / smoothness * modifier) + seed, (y / smoothness * modifier) + seed);

                if (caveValue > 0.5f)
                {
                    underground.Floor.Add(new Vector2Int(x, y));
                    undergroundMap.Add(new Vector2Int(x, y));
                }
                else
                {
                    underground.Cave.Add(new Vector2Int(x, y));
                }
            }

            //// Horní okraj
            //boundaries.Add(new Vector2Int(x, perlinHeight));
            //boundaries.Add(new Vector2Int(x, perlinHeight + 1));
        }

        for (int y = -5; y < height; y++)
        {
            for (int i = -2; i <= 0; i++)
            {
                boundaries.Add(new Vector2Int(startX + i, y));
                boundaries.Add(new Vector2Int(startX + width - 1 - i, y));
            }
        }

        underground.Boundaries = boundaries;
        return undergroundMap;
    }



    private static void SplitVertically(int minWidth, Queue<BoundsInt> objectsQueue, BoundsInt objectBounds)
    {
        // Správné omezení rozsahu splitu
        var xSplit = UnityEngine.Random.Range(minWidth, objectBounds.size.x - minWidth);

        // Rozdělení na dvě části, které jsou dostatečně velké
        BoundsInt object1 = new BoundsInt(objectBounds.min, new Vector3Int(xSplit, objectBounds.size.y, objectBounds.size.z));
        BoundsInt object2 = new BoundsInt(new Vector3Int(objectBounds.min.x + xSplit, objectBounds.min.y, objectBounds.min.z),
            new Vector3Int(objectBounds.size.x - xSplit, objectBounds.size.y, objectBounds.size.z));

        objectsQueue.Enqueue(object1);
        objectsQueue.Enqueue(object2);
    }

    private static void SplitHorizontally(int minHeight, Queue<BoundsInt> objectsQueue, BoundsInt objectBounds)
    {
        // Správné omezení rozsahu splitu
        var ySplit = UnityEngine.Random.Range(minHeight, objectBounds.size.y - minHeight);

        // Rozdělení na dvě části, které jsou dostatečně velké
        BoundsInt object1 = new BoundsInt(objectBounds.min, new Vector3Int(objectBounds.size.x, ySplit, objectBounds.size.z));
        BoundsInt object2 = new BoundsInt(new Vector3Int(objectBounds.min.x, objectBounds.min.y + ySplit, objectBounds.min.z),
            new Vector3Int(objectBounds.size.x, objectBounds.size.y - ySplit, objectBounds.size.z));

        objectsQueue.Enqueue(object1);
        objectsQueue.Enqueue(object2);
    }

}
