using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
