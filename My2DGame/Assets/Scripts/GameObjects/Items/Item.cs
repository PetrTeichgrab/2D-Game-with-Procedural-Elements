using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;


public class Item : MonoBehaviour
{
    public string itemName;
    public Vector2Int Position {  get; set; }

    public void Initialize(string name, Vector2Int pos)
    {
        this.itemName = name;
        this.Position = pos;
    }
}
