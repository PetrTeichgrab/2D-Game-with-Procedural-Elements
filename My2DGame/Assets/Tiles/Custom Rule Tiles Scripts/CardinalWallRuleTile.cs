using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName = "CustomTile/CardinalWallRuleTile")]
public class CardinalWallRuleTile : RuleTile<CardinalWallRuleTile.Neighbor> {

    public bool alwaysConnect;

    public TileBase[] tilesList;

    public class Neighbor : RuleTile.TilingRule.Neighbor {
        public const int Any = 3;
        public const int Floor = 4;
        public const int Nothing = 5;
    }

    public override bool RuleMatch(int neighbor, TileBase tile) {
        switch (neighbor) {
            case Neighbor.This: return CheckThis(tile);
            case Neighbor.NotThis: return CheckNotThis(tile);
            case Neighbor.Any: return CheckAny(tile);
            case Neighbor.Floor: return CheckFloorTile(tile);
            case Neighbor.Nothing: return CheckNothing(tile);

        }
        return base.RuleMatch(neighbor, tile);
    }

    bool CheckThis(TileBase tile)
    {
        if (tile == null)
        {
            return tile == this;
        }
        return tilesList.Contains(tile) || tile == this;
    }

    bool CheckNotThis(TileBase tile)
    {
       return tile != this;
    }

    bool CheckAny(TileBase tile)
    {
       return tile != null;
    }

    bool CheckFloorTile(TileBase tile)
    {
        return tilesList.Contains(tile);
    }

    bool CheckNothing(TileBase tile)
    {
        return tile == null;
    }

}