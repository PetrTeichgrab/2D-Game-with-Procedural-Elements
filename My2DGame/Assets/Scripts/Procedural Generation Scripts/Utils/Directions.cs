using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

//Class for working with Vector directions
public class Directions : MonoBehaviour
{

    //Cardinal directions dictionary
    public static Dictionary<string, Vector2Int> DirectionsDic = new Dictionary<string, Vector2Int>()
    {
        { "top", new Vector2Int(0,1) },
        { "right", new Vector2Int(1,0) },
        { "down", new Vector2Int(-1,0) },
        { "left", new Vector2Int(0,-1) },

    };

    //Diagonal directions dictionary
    public static Dictionary<string, Vector2Int> DiagonalDirectionsDic = new Dictionary<string, Vector2Int>()
    {
        { "top-right", new Vector2Int(1,1) },
        { "top-left", new Vector2Int(-1,1) },
        { "bottom-right", new Vector2Int(1,-1) },
        { "bottom-left", new Vector2Int(-1,-1) },

    };

    //Returns Random cardinal direction
    public static Vector2Int GetRandomDirection()
    {
        return DirectionsDic.Values.ToList()[Random.Range(0, DirectionsDic.Count)];
    }

    //Returns Random diagonal direction
    public static Vector2Int GetRandomDiagonalDirection()
    {
        return DiagonalDirectionsDic.Values.ToList()[Random.Range(0, DirectionsDic.Count)];
    }
}
