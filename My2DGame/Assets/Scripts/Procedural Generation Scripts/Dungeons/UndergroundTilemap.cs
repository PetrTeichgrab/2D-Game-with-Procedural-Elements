using UnityEngine;
using UnityEngine.Tilemaps;

public class UndergroundTilemap : MonoBehaviour
{
    public Tilemap tilemap;

    private void Start()
    {
        if (tilemap == null)
        {
            tilemap = GetComponent<Tilemap>();
        }
        //foreach (var position in tilemap.cellBounds.allPositionsWithin)
        //{
        //    if (tilemap.HasTile(position))
        //    {
        //        Debug.Log($"Tile found at {position}");
        //    }
        //}
    }

    public void BreakTile(Vector3 hitPosition, Vector3 bulletDirection)
    {
        Vector3Int tilePosition = tilemap.WorldToCell(hitPosition);
        Vector3 tileWorldCenter = tilemap.GetCellCenterWorld(tilePosition);

        //Debug.Log($"Hit position: {hitPosition}");
        //Debug.Log($"Converted tile position: {tilePosition}");
        //Debug.Log($"Tile world center: {tileWorldCenter}");

        if (tilemap.HasTile(tilePosition))
        {
            DestroyTile(tilePosition);
        }
        else
        {
            Debug.Log($"No tile found at {tilePosition}. Checking nearby positions.");

            Vector3Int bestTilePosition = FindBestTilePosition(tilePosition, bulletDirection);

            if (bestTilePosition != tilePosition)
            {
                DestroyTile(bestTilePosition);
            }
        }
    }


    private void DestroyTile(Vector3Int tilePosition)
    {
        tilemap.SetTile(tilePosition, null);
        Debug.Log($"Tile at {tilePosition} destroyed.");
    }

    private Vector3Int FindBestTilePosition(Vector3Int tilePosition, Vector3 bulletDirection)
    {
        Vector3Int preferredDirection = new Vector3Int(
            Mathf.RoundToInt(bulletDirection.x),
            Mathf.RoundToInt(bulletDirection.y),
            0
        );

        Vector3Int[] directions = {
            preferredDirection,
            new Vector3Int(preferredDirection.x, 0, 0),
            new Vector3Int(0, preferredDirection.y, 0),
            Vector3Int.right, Vector3Int.left, Vector3Int.up, Vector3Int.down
        };

        foreach (var dir in directions)
        {
            Vector3Int neighborPos = tilePosition + dir;
            if (tilemap.HasTile(neighborPos))
            {
                return neighborPos;
            }
        }

        return tilePosition;
    }

    public void PrintTilemap()
    {
        BoundsInt bounds = tilemap.cellBounds;
        for (int x = bounds.xMin; x < bounds.xMax; x++)
        {
            for (int y = bounds.yMin; y < bounds.yMax; y++)
            {
                Vector3Int tilePosition = new Vector3Int(x, y, 0);
                if (tilemap.HasTile(tilePosition))
                {
                    Debug.Log($"Tile found at {tilePosition}");
                }
            }
        }
    }
}
