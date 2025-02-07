using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class PinkDungeon : DungeonBehaviour
{
    [SerializeField]
    DungeonGenerator generator;

    [SerializeField]
    private EnemyMushroomPink pinkMushroom;

    [SerializeField]
    private EnemyMushroomPinkBoss pinkMushroomBoss;

    [SerializeField]
    Item pinkCrystal;

    [SerializeField]
    Item barel;

    [SerializeField]
    Item pinkStatue;

    [SerializeField]
    Item pinkRock1small;

    [SerializeField]
    Item pinkRock1med;

    [SerializeField]
    Item pinkRock2large;

    [SerializeField]
    Item grassSmall;

    [SerializeField]
    Item grassMed;

    [SerializeField]
    Item grassLarge;

    [SerializeField]
    PinkColorCore pinkColorCore;

    private EnemyMushroomPinkBoss PinkMushroomBossInstance { get; set; }

    Dungeon pinkDungeon;

    private void Update()
    {
        if (PinkMushroomBossInstance != null)
        {
            if (!PinkMushroomBossInstance.isAlive && !Completed)
            {
                StartCoroutine(CallSpawnColorCoreAfterDelay(1.2f, pinkColorCore, PinkMushroomBossInstance.transform));
                Completed = true;
            }
        }
    }

    public override void Create()
    {
        pinkDungeon = generator.PinkDungeon;
        CreateAndSetPositions();
    }

    public override void CreateAndSetPositions()
    {

        // Inicializace hlavního bosse
        PinkMushroomBossInstance = Instantiate(this.pinkMushroomBoss, this.pinkMushroom.transform.position,
                this.pinkMushroom.transform.rotation);
        generator.setBossToRandomRoom(PinkMushroomBossInstance, pinkDungeon, 2, 2);
        generator.Player.transform.position = new Vector3(PinkMushroomBossInstance.Position.x + 5, PinkMushroomBossInstance.Position.y + 5);

        // Seznam konfigurací pro generování objektù specifikovaných typem Item
        var itemConfigs = new List<(Item prefab, Action<Item> positionSetter, int count)>()
        {
            //(this.pinkRock2large, obj => generator.SetLargeItemToRoomCenter(obj, pinkDungeon, 3, 3), 2),
            (this.pinkCrystal, obj => generator.SetItemToRandomPosition(obj, pinkDungeon, 3), 10),
            (this.pinkRock1small, obj => generator.SetItemToEdgeOfRoom(obj, pinkDungeon), 5),
            (this.pinkRock1small, obj => generator.SetItemToRandomPosition(obj, pinkDungeon, 3), 10),
            (this.pinkRock1med, obj => generator.SetItemToRandomPosition(obj, pinkDungeon, 3), 7),
            (this.grassSmall, obj => generator.SetItemToRandomPosition(obj, pinkDungeon, 0), 30),
            (this.grassMed, obj => generator.SetItemToRandomPosition(obj, pinkDungeon, 0), 40),
            (this.grassLarge, obj => generator.SetItemToRandomPosition(obj, pinkDungeon, 0), 50)
        };

        var enemyConfigs = new List<(EnemyMushroomPink prefab, Action<EnemyMushroomPink> positionSetter, int count)>()
    {
        (this.pinkMushroom, obj => generator.setCharacterToRandomPosition(obj, pinkDungeon, 0), 30),
    };

        GenerateDungeonObjects(itemConfigs, enemyConfigs);
    }
}
