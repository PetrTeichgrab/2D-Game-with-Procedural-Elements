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
    private Tilemap undergroundMap;

    [SerializeField]
    private GameObject backWall;

    [SerializeField]
    private Tilemap colliderMap;

    [SerializeField]
    private TileBase emptyTile;

    [SerializeField]
    private TileBase pinkFloorTile, crackedPinkFloor1, crackedPinkFloor2, crackedPinkFloor3;


    [SerializeField]
    private TileBase blueFloorTile, crackedBlueFloor1, crackedBlueFloor2, crackedBlueFloor3;

    [SerializeField]
    private TileBase greenFloorTile, crackedGreenFloor1, crackedGreenFloor2, crackedGreenFloor3;

    [SerializeField]
    private TileBase purpleFloorTile, crackedPurpleFloor1, crackedPurpleFloor2, crackedPurpleFloor3;


    [SerializeField]
    private TileBase pinkWallTop, 
            pinkSideWallRight, 
            pinkSideWallLeft, 
            pinkWallBottom, 
            pinkFullWall, 
            pinkWallBottomCornerLeft, 
            pinkWallBottomCornerRight,
            pinkDiagonalCornerBottomRight, 
            pinkDiagonalCornerBottomLeft, 
            pinkDiagonalCornerUpRight, 
            pinkDiagonalCornerUpLeft;

    [SerializeField]
    private TileBase blueWallTop,
            blueSideWallRight,
            blueSideWallLeft,
            blueWallBottom,
            blueFullWall,
            blueWallBottomCornerLeft,
            blueWallBottomCornerRight,
            blueDiagonalCornerBottomRight,
            blueDiagonalCornerBottomLeft,
            blueDiagonalCornerUpRight,
            blueDiagonalCornerUpLeft;
    [SerializeField]
    private TileBase redWallTop,
            redSideWallRight,
            redSideWallLeft,
            redWallBottom,
            redFullWall,
            redWallBottomCornerLeft,
            redWallBottomCornerRight,
            redDiagonalCornerBottomRight,
            redDiagonalCornerBottomLeft,
            redDiagonalCornerUpRight,
            redDiagonalCornerUpLeft;
    [SerializeField]
    private TileBase greenWallTop,
            greenSideWallRight,
            greenSideWallLeft,
            greenWallBottom,
            greenFullWall,
            greenWallBottomCornerLeft,
            greenWallBottomCornerRight,
            greenDiagonalCornerBottomRight,
            greenDiagonalCornerBottomLeft,
            greenDiagonalCornerUpRight,
            greenDiagonalCornerUpLeft;
    [SerializeField]
    private TileBase purpleWallTop,
        purpleSideWallRight,
        purpleSideWallLeft,
        purpleWallBottom,
        purpleFullWall,
        purpleWallBottomCornerLeft,
        purpleWallBottomCornerRight,
        purpleDiagonalCornerBottomRight,
        purpleDiagonalCornerBottomLeft,
        purpleDiagonalCornerUpRight,
        purpleDiagonalCornerUpLeft;

    [SerializeField]
    private TileBase redUndergroundFloorTile, redUndergroundGroundTile;

    public void DrawFloor(Dungeon dungeon)
    {
        foreach (Vector2Int floor in dungeon.Floor.FloorList)
        {
            switch (dungeon.Color)
            {
                case DungeonColor.Pink:
                    DrawTilesRandomly(this.pinkFloorTile, this.crackedPinkFloor1, this.crackedPinkFloor2, this.crackedPinkFloor3, floor);
                    break;
                case DungeonColor.Blue:
                    DrawTilesRandomly(this.blueFloorTile, this.crackedBlueFloor1, this.crackedBlueFloor2, this.crackedBlueFloor3, floor);
                    break;
                case DungeonColor.Green:
                    DrawTilesRandomly(this.greenFloorTile, this.crackedGreenFloor1, this.crackedGreenFloor2, this.crackedGreenFloor3, floor);
                    break;
                case DungeonColor.Purple:
                    DrawTilesRandomly(this.purpleFloorTile, this.crackedPurpleFloor1, this.crackedPurpleFloor2, this.crackedPurpleFloor3, floor);
                    break;
            }
        
        }
    }

    public void DrawUndergroundFloor(Underground underground)
    {
        foreach(var position in underground.Floor){
            Vector3Int tilePosition = this.map.WorldToCell((Vector3Int)position);
            this.undergroundMap.SetTile(tilePosition, redUndergroundFloorTile);
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

    public void DrawCardinalWalls(HashSet<Vector2Int> wallPositions, HashSet<Vector2Int> floorPositions,  DungeonColor color)
    {
        foreach (Vector2Int wall in wallPositions)
        {
            string neighbourBinary = "";
            foreach (var direction in Directions.CardinalDirectionsDic.Values)
            {
                var neighbourPosition = wall + direction;
                if (floorPositions.Contains(neighbourPosition))
                {
                    neighbourBinary += "1";
                }
                else
                {
                    neighbourBinary += "0";
                }
                DrawCardinalWallTile(wall, neighbourBinary, color);
            }
        
        }
    }

    public void DrawDiagonalWalls(HashSet<Vector2Int> wallPositions, HashSet<Vector2Int> floorPositions, DungeonColor color)
    {
        foreach (Vector2Int wall in wallPositions)
        {
            string neighbourBinary = "";
            foreach (var direction in Directions.AllDirectionsDic.Values)
            {
                var neighbourPosition = wall + direction;
                if (floorPositions.Contains(neighbourPosition))
                {
                    neighbourBinary += "1";
                }
                else
                {
                    neighbourBinary += "0";
                }
                DrawDiagonalWallTile(wall, neighbourBinary, color);
            }
        }
    }

    public void DrawTile(TileBase tile, Vector2Int position)
    {
        Vector3Int tilePosition = this.map.WorldToCell((Vector3Int)position);
        this.map.SetTile(tilePosition, tile);
    }

    public void DrawCardinalWallTile(Vector2Int position, string binary, DungeonColor color)
    {
        int binaryNumber = Convert.ToInt32(binary, 2);
        TileBase tile = null;
        if (WallByteTypes.wallTop.Contains(binaryNumber))
        {
            switch (color)
            {
                case DungeonColor.Pink:
                    tile = pinkWallTop;
                    break;
                case DungeonColor.Blue:
                    tile = blueWallTop;
                    break;
                case DungeonColor.Green:
                    tile = greenWallTop;
                    break;
                case DungeonColor.Purple:
                    tile = purpleWallTop;
                    break;
            }
        }
        else if (WallByteTypes.wallSideRight.Contains(binaryNumber))
        {
            switch (color)
            {
                case DungeonColor.Pink:
                    tile = pinkSideWallRight;
                    break;
                case DungeonColor.Blue:
                    tile = blueSideWallRight;
                    break;
                case DungeonColor.Green:
                    tile = greenSideWallRight;
                    break;
                case DungeonColor.Purple:
                    tile = purpleSideWallRight;
                    break;
            }
        }
        else if (WallByteTypes.wallSideLeft.Contains(binaryNumber))
        {
            switch (color)
            {
                case DungeonColor.Pink:
                    tile = pinkSideWallLeft;
                    break;
                case DungeonColor.Blue:
                    tile = blueSideWallLeft;
                    break;
                case DungeonColor.Green:
                    tile = greenSideWallLeft;
                    break;
                case DungeonColor.Purple:
                    tile = purpleSideWallLeft;
                    break;
            }
        }
        else if (WallByteTypes.wallBottom.Contains(binaryNumber))
        {
            switch (color)
            {
                case DungeonColor.Pink:
                    tile = pinkWallBottom;
                    break;
                case DungeonColor.Blue:
                    tile = blueWallBottom;
                    break;
                case DungeonColor.Green:
                    tile = greenWallBottom;
                    break;
                case DungeonColor.Purple:
                    tile = purpleWallBottom;
                    break;
            }
        }
        else if (WallByteTypes.wallBottomCornerLeft.Contains(binaryNumber))
        {
            switch (color)
            {
                case DungeonColor.Pink:
                    tile = pinkWallBottomCornerLeft;
                    break;
                case DungeonColor.Blue:
                    tile = blueWallBottomCornerLeft;
                    break;
                case DungeonColor.Green:
                    tile = greenWallBottomCornerLeft;
                    break;
                case DungeonColor.Purple:
                    tile = purpleWallBottomCornerLeft;
                    break;
            }
        }
        else if (WallByteTypes.wallBottomCornerRight.Contains(binaryNumber))
        {
            switch (color)
            {
                case DungeonColor.Pink:
                    tile = pinkWallBottomCornerRight;
                    break;
                case DungeonColor.Blue:
                    tile = blueWallBottomCornerRight;
                    break;
                case DungeonColor.Green:
                    tile = greenWallBottomCornerRight;
                    break;
                case DungeonColor.Purple:
                    tile = purpleWallBottomCornerRight;
                    break;
            }
        }
        else if (WallByteTypes.wallFull.Contains(binaryNumber))
        {
            switch (color)
            {
                case DungeonColor.Pink:
                    tile = pinkFullWall;
                    break;
                case DungeonColor.Blue:
                    tile = blueFullWall;
                    break;
                case DungeonColor.Green:
                    tile = greenFullWall;
                    break;
                case DungeonColor.Purple:
                    tile = purpleFullWall;
                    break;
            }
        }
        Vector3Int tilePosition = this.map.WorldToCell((Vector3Int)position);
        this.map.SetTile(tilePosition, tile);
    }

    public void DrawDiagonalWallTile(Vector2Int position, string binary, DungeonColor color)
    {
        int binaryNumber = Convert.ToInt32(binary, 2);
        TileBase tile = null;
        if (WallByteTypes.wallInnerCornerDownLeft.Contains(binaryNumber))
        {
            switch (color)
            {
                case DungeonColor.Pink:
                    tile = pinkWallBottomCornerLeft;
                    break;
                case DungeonColor.Blue:
                    tile = blueWallBottomCornerLeft;
                    break;
                case DungeonColor.Green:
                    tile = greenWallBottomCornerLeft;
                    break;
                case DungeonColor.Purple:
                    tile = purpleWallBottomCornerLeft;
                    break;
            }
        }
        else if (WallByteTypes.wallInnerCornerDownRight.Contains(binaryNumber))
        {
            switch (color)
            {
                case DungeonColor.Pink:
                    tile = pinkWallBottomCornerRight;
                    break;
                case DungeonColor.Blue:
                    tile = blueWallBottomCornerRight;
                    break;
                case DungeonColor.Green:
                    tile = greenWallBottomCornerRight;
                    break;
                case DungeonColor.Purple:
                    tile = purpleWallBottomCornerRight;
                    break;
            }
        }
        else if (WallByteTypes.wallDiagonalCornerDownLeft.Contains(binaryNumber))
        {
            switch (color)
            {
                case DungeonColor.Pink:
                    tile = pinkDiagonalCornerBottomLeft;
                    break;
                case DungeonColor.Blue:
                    tile = blueDiagonalCornerBottomLeft;
                    break;
                case DungeonColor.Green:
                    tile = greenDiagonalCornerBottomLeft;
                    break;
                case DungeonColor.Purple:
                    tile = purpleDiagonalCornerBottomLeft;
                    break;
            }
        }
        else if (WallByteTypes.wallDiagonalCornerDownRight.Contains(binaryNumber))
        {
            switch (color)
            {
                case DungeonColor.Pink:
                    tile = pinkDiagonalCornerBottomRight;
                    break;
                case DungeonColor.Blue:
                    tile = blueDiagonalCornerBottomRight;
                    break;
                case DungeonColor.Green:
                    tile = greenDiagonalCornerBottomRight;
                    break;
                case DungeonColor.Purple:
                    tile = purpleDiagonalCornerBottomRight;
                    break;
            }
        }
        else if (WallByteTypes.wallDiagonalCornerUpRight.Contains(binaryNumber))
        {
            switch (color)
            {
                case DungeonColor.Pink:
                    tile = pinkDiagonalCornerUpRight;
                    break;
                case DungeonColor.Blue:
                    tile = blueDiagonalCornerUpRight;
                    break;
                case DungeonColor.Green:
                    tile = greenDiagonalCornerUpRight;
                    break;
                case DungeonColor.Purple:
                    tile = purpleDiagonalCornerUpRight;
                    break;
            }
        }
        else if (WallByteTypes.wallDiagonalCornerUpLeft.Contains(binaryNumber))
        {
            switch (color)
            {
                case DungeonColor.Pink:
                    tile = pinkDiagonalCornerUpLeft;
                    break;
                case DungeonColor.Blue:
                    tile = blueDiagonalCornerUpLeft;
                    break;
                case DungeonColor.Green:
                    tile = greenDiagonalCornerUpLeft;
                    break;
                case DungeonColor.Purple:
                    tile = purpleDiagonalCornerUpLeft;
                    break;
            }
        }
        else if (WallByteTypes.wallFullEightDirections.Contains(binaryNumber))
        {
            switch (color)
            {
                case DungeonColor.Pink:
                    tile = pinkFullWall;
                    break;
                case DungeonColor.Blue:
                    tile = blueFullWall;
                    break;
                case DungeonColor.Green:
                    tile = greenFullWall;
                    break;
                case DungeonColor.Purple:
                    tile = purpleFullWall;
                    break;
            }
        }
        else if (WallByteTypes.wallBottmEightDirections.Contains(binaryNumber))
        {
            switch (color)
            {
                case DungeonColor.Pink:
                    tile = pinkWallBottom;
                    break;
                case DungeonColor.Blue:
                    tile = blueWallBottom;
                    break;
                case DungeonColor.Green:
                    tile = greenWallBottom;
                    break;
                case DungeonColor.Purple:
                    tile = purpleWallBottom;
                    break;
            }
        }
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
        undergroundMap.ClearAllTiles();
        map.ClearAllTiles();
        colliderMap.ClearAllTiles();
    }
}
