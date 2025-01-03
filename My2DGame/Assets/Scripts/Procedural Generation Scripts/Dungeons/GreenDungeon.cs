using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenDungeon : DungeonBehaviour
{
    [SerializeField]
    DungeonGenerator generator;

    [SerializeField]
    GreenSlime greenSlime;

    //[SerializeField]
    //GreenBossSlime greenBossSlime;

    [SerializeField]
    Item treeBright1;

    [SerializeField]
    Item treeBright2;

    [SerializeField]
    Item treeBright3;

    [SerializeField]
    Item treeBright4;

    [SerializeField]
    Item treeBright5;

    [SerializeField]
    Item treeBright6;

    [SerializeField]
    Item treeBright7;

    [SerializeField]
    Item treeDark1;

    [SerializeField]
    Item treeDark2;

    [SerializeField]
    Item treeDark3;

    [SerializeField]
    Item treeDark4;

    [SerializeField]
    Item treeDark5;

    [SerializeField]
    Item treeDark6;

    [SerializeField]
    Item treeDark7;

    [SerializeField]
    Item bush1;

    [SerializeField]
    Item bush2;

    [SerializeField]
    Item bush3;

    [SerializeField]
    Item bush4;

    [SerializeField]
    Item bush5;

    [SerializeField]
    Item bush6;

    [SerializeField]
    Item bush7;

    [SerializeField]
    Item bush8;

    [SerializeField]
    Item bush9;

    [SerializeField]
    Item bush10;

    Dungeon greenDungeon;

    public override void Create()
    {
        greenDungeon = generator.GreenDungeon;
        CreateAndSetPositions();
    }

    public override void CreateAndSetPositions()
    {
        // Seznam konfigurací pro generování objektù specifikovaných typem Item
        generator.Player.transform.position = new Vector3(greenDungeon.RoomList[0].Center.x, greenDungeon.RoomList[0].Center.y);

        var itemConfigs = new List<(Item prefab, Action<Item> positionSetter, int count)>()
        {
            (this.bush1, obj => generator.SetItemToRandomPosition(obj, greenDungeon, 1), 30),
            (this.bush2, obj => generator.SetItemToRandomPosition(obj, greenDungeon, 1), 30),
            (this.bush3, obj => generator.SetItemToRandomPosition(obj, greenDungeon, 1), 30),
            (this.bush4, obj => generator.SetItemToRandomPosition(obj, greenDungeon, 1), 30),
            (this.bush5, obj => generator.SetItemToRandomPosition(obj, greenDungeon, 1), 30),
            (this.bush6, obj => generator.SetItemToRandomPosition(obj, greenDungeon, 1), 30),
            (this.bush7, obj => generator.SetItemToRandomPosition(obj, greenDungeon, 1), 30),
            (this.bush8, obj => generator.SetItemToRandomPosition(obj, greenDungeon, 1), 30),
            (this.bush9, obj => generator.SetItemToRandomPosition(obj, greenDungeon, 1), 30),
            (this.bush10, obj => generator.SetItemToRandomPosition(obj, greenDungeon, 1), 30),
            (this.treeBright1, obj => generator.SetLargeItemToRandomPosition(obj, greenDungeon, 1, 2, 2), 80),
            (this.treeBright2, obj => generator.SetLargeItemToRandomPosition(obj, greenDungeon, 2, 2, 2), 80),
            (this.treeBright3, obj => generator.SetLargeItemToRandomPosition(obj, greenDungeon, 2, 2, 2), 80),
            (this.treeBright4, obj => generator.SetLargeItemToRandomPosition(obj, greenDungeon, 2, 2, 2), 80),
            (this.treeBright5, obj => generator.SetLargeItemToRandomPosition(obj, greenDungeon, 2, 2, 2), 80),
            (this.treeBright6, obj => generator.SetLargeItemToRandomPosition(obj, greenDungeon, 2, 2, 2), 80),
        };

        var enemyConfigs = new List<(GreenSlime prefab, Action<GreenSlime> positionSetter, int count)>()
        {
            (this.greenSlime, obj => generator.setCharacterToRandomPosition(obj, greenDungeon, 0), 30),
        };

        GenerateDungeonObjects(itemConfigs, enemyConfigs);
    }
}
