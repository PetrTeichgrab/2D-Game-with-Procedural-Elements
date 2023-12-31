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
    private TileBase pinkFloorTile;

    [SerializeField]
    private TileBase blueFloorTile;

    [SerializeField]
    private RuleTile cardinalPinkWallRuleTile;

    [SerializeField]
    private RuleTile cardinalBlueWallRuleTile;

    [SerializeField]
    private RuleTile diagonalPinkWallRuleTile;

    [SerializeField]
    private RuleTile diagonalBlueWallRuleTile;

    public void DrawFloor(HashSet<Vector2Int> floorPositions, Color color)
    {
        foreach (Vector2Int floor in floorPositions)
        {
            switch (color)
            {
                case Color.Pink:
                    DrawTile(this.pinkFloorTile, floor);
                    break;
                case Color.Blue:
                    DrawTile(this.blueFloorTile, floor);
                    break;
            }
        
        }
    }

    public void DrawCardinalWalls(HashSet<Vector2Int> wallPositions, Color color)
    {
        foreach (Vector2Int wall in wallPositions)
        {
            switch (color)
            {
                case Color.Pink:
                    DrawTile(this.cardinalPinkWallRuleTile, wall);
                    break;
                case Color.Blue:
                    DrawTile(this.cardinalBlueWallRuleTile, wall);
                    break;
            }
        
        }
    }

    public void DrawDiagonalWalls(HashSet<Vector2Int> wallPositions, Color color)
    {
        foreach (Vector2Int wall in wallPositions)
        {
            switch (color)
            {
                case Color.Pink:
                    DrawTile(this.diagonalPinkWallRuleTile, wall);
                    break;
                case Color.Blue:
                    DrawTile(this.diagonalBlueWallRuleTile, wall);
                    break;
            }

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
