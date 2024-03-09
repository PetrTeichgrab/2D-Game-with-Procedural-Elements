using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

//Class for working with Vector directions
public class Directions : MonoBehaviour
{

    //Cardinal directions dictionary
    public static Dictionary<string, Vector2Int> CardinalDirectionsDic = new Dictionary<string, Vector2Int>()
    {
        { "top", new Vector2Int(0,1) },
        { "right", new Vector2Int(1,0) },
        { "down", new Vector2Int(0,-1) },
        { "left", new Vector2Int(-1,0) },

    };

    //Diagonal directions dictionary
    public static Dictionary<string, Vector2Int> DiagonalDirectionsDic = new Dictionary<string, Vector2Int>()
    {
        { "top-right", new Vector2Int(1,1) },
        { "bottom-right", new Vector2Int(1,-1) },
        { "bottom-left", new Vector2Int(-1,-1) },
        { "top-left", new Vector2Int(-1,1) },

    };

    public static Dictionary<string, Vector2Int> AllDirectionsDic = new Dictionary<string, Vector2Int>()
    {
        { "top", new Vector2Int(0,1) },
        { "top-right", new Vector2Int(1,1) },
        { "right", new Vector2Int(1,0) },
        { "bottom-right", new Vector2Int(1,-1) },
        { "down", new Vector2Int(0,-1) },
        { "bottom-left", new Vector2Int(-1,-1) },
        { "left", new Vector2Int(-1,0) },
        { "top-left", new Vector2Int(-1,1) },

    };

    public static List<Vector2Int> cardinalDirectionsList = new List<Vector2Int>
    {
        new Vector2Int(0,1), //UP
        new Vector2Int(1,0), //RIGHT
        new Vector2Int(0, -1), // DOWN
        new Vector2Int(-1, 0) //LEFT
    };

    public static List<Vector2Int> diagonalDirectionsList = new List<Vector2Int>
    {
        new Vector2Int(1,1), //UP-RIGHT
        new Vector2Int(1,-1), //RIGHT-DOWN
        new Vector2Int(-1, -1), // DOWN-LEFT
        new Vector2Int(-1, 1) //LEFT-UP
    };

    public static List<Vector2Int> eightDirectionsList = new List<Vector2Int>
    {
        new Vector2Int(0,1), //UP
        new Vector2Int(1,1), //UP-RIGHT
        new Vector2Int(1,0), //RIGHT
        new Vector2Int(1,-1), //RIGHT-DOWN
        new Vector2Int(0, -1), // DOWN
        new Vector2Int(-1, -1), // DOWN-LEFT
        new Vector2Int(-1, 0), //LEFT
        new Vector2Int(-1, 1) //LEFT-UP

    };

    //Returns Random cardinal direction
    public static Vector2Int GetRandomCardinalDirection()
    {
        return CardinalDirectionsDic.Values.ToList()[Random.Range(0, CardinalDirectionsDic.Count)];
    }


    public static void Shuffle(List<Vector2Int> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = Random.Range(0, n + 1);
            var value = list[k];
            list[k] = list[n];
            list[n] = value;
        }

    }

    public static List<Vector2Int> GetRandomDirectionsList()
    {
        Shuffle(CardinalDirectionsDic.Values.ToList());
        return CardinalDirectionsDic.Values.ToList();
    }

    //Returns Random diagonal direction
    public static Vector2Int GetRandomDiagonalDirection()
    {
        return DiagonalDirectionsDic.Values.ToList()[Random.Range(0, CardinalDirectionsDic.Count)];
    }
}
