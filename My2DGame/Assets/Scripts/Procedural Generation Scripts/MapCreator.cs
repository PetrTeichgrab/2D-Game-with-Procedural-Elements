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
    private Tilemap colliderMap;

    [SerializeField]
    private TileBase emptyTile;

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

    public void DrawFloor(IDungeon dungeon)
    {
        foreach (Vector2Int floor in dungeon.Floor.FloorList)
        {
            switch (dungeon.Color)
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
                    AddCollider(emptyTile, wall);
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

    public void DrawTile(TileBase tile, Vector2Int position)
    {
        Vector3Int tilePosition = this.map.WorldToCell((Vector3Int)position);
        this.map.SetTile(tilePosition, tile);
    }

    public void AddCollider(TileBase tile, Vector2Int position)
    {
        Vector3Int tilePosition = this.map.WorldToCell((Vector3Int)position);
        this.colliderMap.SetTile(tilePosition, tile);
    }

    public void ClearGeneration()
    {
        map.ClearAllTiles();
        colliderMap.ClearAllTiles();
    }
}
