using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Directions : MonoBehaviour
{
    public static Dictionary<string, Vector2Int> DirectionsDic = new Dictionary<string, Vector2Int>()
    {
        { "top", new Vector2Int(0,1) },
        { "right", new Vector2Int(1,0) },
        { "down", new Vector2Int(-1,0) },
        { "left", new Vector2Int(0,-1) },

    };

    public static Dictionary<string, Vector2Int> DiagonalDirectionsDic = new Dictionary<string, Vector2Int>()
    {
        { "top-right", new Vector2Int(1,1) },
        { "top-left", new Vector2Int(-1,1) },
        { "bottom-right", new Vector2Int(1,-1) },
        { "bottom-left", new Vector2Int(-1,-1) },

    };

    public static Vector2Int GetRandomDirection()
    {
        return DirectionsDic.Values.ToList()[Random.Range(0, DirectionsDic.Count)];
    }
    public static Vector2Int GetRandomDiagonalDirection()
    {
        return DiagonalDirectionsDic.Values.ToList()[Random.Range(0, DirectionsDic.Count)];
    }
}
