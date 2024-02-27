using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;
using UnityEngine.WSA;

public class MapCreator : MonoBehaviour
{
    [SerializeField]
    private Tilemap map;

    [SerializeField]
    private GameObject backWall;

    [SerializeField]
    private Tilemap colliderMap;

    [SerializeField]
    private TileBase emptyTile;

    [SerializeField]
    private TileBase crackedPinkFloor1;

    [SerializeField]
    private TileBase crackedPinkFloor2;

    [SerializeField]
    private TileBase crackedPinkFloor3;

    [SerializeField]
    private TileBase crackedBlueFloor1;

    [SerializeField]
    private TileBase crackedBlueFloor2;

    [SerializeField]
    private TileBase crackedBlueFloor3;

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

    public void DrawFloor(Dungeon dungeon)
    {
        foreach (Vector2Int floor in dungeon.Floor.FloorList)
        {
            switch (dungeon.Color)
            {
                case Color.Pink:
                    DrawTilesRandomly(this.pinkFloorTile, this.crackedPinkFloor1, this.crackedPinkFloor2, this.crackedPinkFloor3, floor);
                    break;
                case Color.Blue:
                    DrawTilesRandomly(this.blueFloorTile, this.crackedBlueFloor1, this.crackedBlueFloor2, this.crackedBlueFloor3, floor);
                    break;
            }
        
        }
    }

    private void DrawTilesRandomly(TileBase tile1, TileBase tile2, TileBase tile3, TileBase tile4, Vector2Int position)
    {
        System.Random rnd= new System.Random();
        int random = rnd.Next(0,101);
        if(random <= 97)
        {
           DrawTile(tile1, position);
            return;
        }
        else if(random == 98) { 
           DrawTile(tile2, position);
            return;
        }
        else if (random == 99)
        {
           DrawTile(tile3, position);
            return;
        }
        else if (random == 100)
        {
           DrawTile(tile4, position);
            return;
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
