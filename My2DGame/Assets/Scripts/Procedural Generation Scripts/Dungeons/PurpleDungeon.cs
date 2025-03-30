using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PurpleDungeon : DungeonBehaviour
{
    [SerializeField]
    DungeonGenerator generator;

    Dungeon purpleDungeon;

    private void Update()
    {

    }

    public override void Create()
    {
        purpleDungeon = generator.PurpleDungeon;
        CreateAndSetPositions();
    }

    public override void CreateAndSetPositions()
    {
        portal = Instantiate(portalPrefab);
        generator.SetLargeItemToRandomPosition(portal, purpleDungeon, 2, 2, 1);
        var itemConfigs = new List<(Item prefab, Action<Item> positionSetter, int count)>()
        {
        };

        var enemyConfigs = new List<(Character prefab, Action<Character> positionSetter)>()
        {
        };

        GenerateObjects(itemConfigs);
    }
}

