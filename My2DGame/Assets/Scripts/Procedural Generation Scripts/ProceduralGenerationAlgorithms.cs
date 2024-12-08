using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEditor.UI;
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
            if (toBeSplitObject.size.y >= minHeight && toBeSplitObject.size.x >= minWidth)
            {
                if (UnityEngine.Random.value < 0.5f)
                {
                    if (toBeSplitObject.size.y >= minHeight * 2)
                    {
                        SplitHorizontally(minWidth, toBeSplitObjects, toBeSplitObject);
                    }
                    else if (toBeSplitObject.size.x >= minWidth * 2)
                    {
                        SplitVertically(minHeight, toBeSplitObjects, toBeSplitObject);
                    }
                    else
                    {
                        splittedObjects.Add(toBeSplitObject);
                    }
                }
                else
                {
                    if (toBeSplitObject.size.x >= minWidth * 2)
                    {
                        SplitVertically(minWidth, toBeSplitObjects, toBeSplitObject);
                    }
                    else if (toBeSplitObject.size.y >= minHeight * 2)
                    {
                        SplitHorizontally(minHeight, toBeSplitObjects, toBeSplitObject);
                    }
                    else
                    {
                        splittedObjects.Add(toBeSplitObject);
                    }
                }
            }
        }
        return splittedObjects;
    }

    public static HashSet<Vector2Int> PerlinNoise(Underground underground, int height, int width, float smoothness, float modifier, int startX = 0)
    {
        int perlinHeight = 0;
        int seed = UnityEngine.Random.Range(-100000, 100000);
        HashSet<Vector2Int> undergroundMap = new HashSet<Vector2Int>();

        for (int x = startX; x < width + startX; x++)
        {
            float perlinValue = Mathf.PerlinNoise((x / smoothness) + seed, seed);
            perlinHeight = Mathf.RoundToInt(perlinValue * (height / 2)) + (height / 2);
            underground.Floor.Add(new Vector2Int(x, 0));

            for (int y = 0; y < perlinHeight; y++)
            {
                float caveValue = Mathf.PerlinNoise((x / smoothness * modifier) + seed, (y / smoothness * modifier) + seed);

                if (caveValue > 0.5f)
                {
                    underground.Floor.Add(new Vector2Int(x, y));
                }
            }
        }

        return undergroundMap;
    }




    private static void SplitVertically(int minWidth, Queue<BoundsInt> objectsQueue, BoundsInt objectBounds)
    {
        var xSplit = UnityEngine.Random.Range(1, objectBounds.size.x);
        BoundsInt object1 = new BoundsInt(objectBounds.min, new Vector3Int(xSplit, objectBounds.size.y, objectBounds.size.z));
        BoundsInt object2 = new BoundsInt(new Vector3Int(objectBounds.min.x + xSplit, objectBounds.min.y, objectBounds.min.z),
            new Vector3Int(objectBounds.size.x - xSplit, objectBounds.size.y, objectBounds.size.z));
        objectsQueue.Enqueue(object1);
        objectsQueue.Enqueue(object2);
    }

    private static void SplitHorizontally(int minHeight, Queue<BoundsInt> objectsQueue, BoundsInt objectBounds)
    {
        var ySplit = UnityEngine.Random.Range(1, objectBounds.size.y);
        BoundsInt object1 = new BoundsInt(objectBounds.min, new Vector3Int(objectBounds.size.x, ySplit, objectBounds.size.z));
        BoundsInt object2 = new BoundsInt(new Vector3Int(objectBounds.min.x, objectBounds.min.y + ySplit, objectBounds.min.z),
            new Vector3Int(objectBounds.size.x, objectBounds.size.y - ySplit, objectBounds.size.z));
        objectsQueue.Enqueue(object1);
        objectsQueue.Enqueue(object2);
    }
}
