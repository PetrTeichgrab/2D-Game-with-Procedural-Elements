using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;
using UnityEngine.WSA;

public class MapCreator : MonoBehaviour
{
    [SerializeField]
    private Tilemap map;

    [SerializeField]
    private TileBase floorTile;

    [SerializeField]
    private RuleTile wallRuleTile;

    [SerializeField]
    private RuleTile diagonalWallRuleTile;

    public void DrawFloor(HashSet<Vector2Int> floorPositions)
    {
        foreach (Vector2Int floor in floorPositions)
        {
            DrawTile(this.floorTile, floor);
        }
    }

    public void DrawCardinalWalls(HashSet<Vector2Int> wallPositions)
    {
        foreach (Vector2Int wall in wallPositions)
        {
            DrawTile(this.wallRuleTile, wall);
        }
    }

    public void DrawDiagonalWalls(HashSet<Vector2Int> wallPositions)
    {
        foreach (Vector2Int wall in wallPositions)
        {
            DrawTile(this.diagonalWallRuleTile, wall);
        }
    }

    public void DrawTile(TileBase tile, Vector2Int floorPosition)
    {
        Vector3Int tilePosition = this.map.WorldToCell((Vector3Int)floorPosition);
        this.map.SetTile(tilePosition, tile);
    }

    public void ClearGeneration()
    {
        map.ClearAllTiles();
    }
}
