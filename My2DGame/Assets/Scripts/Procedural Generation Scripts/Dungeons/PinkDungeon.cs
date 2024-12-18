using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PinkDungeon : MonoBehaviour
{
    [SerializeField]
    DungeonGenerator generator;

    [SerializeField]
    public EnemyMushroomPink pinkMushroom;

    [SerializeField]
    public EnemyMushroomPinkBoss pinkMushroomBoss;

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

    Dungeon pinkDungeon;


    private bool completed { get; set; } = false;

    private void Update()
    {
        if (!pinkMushroomBoss.isAlive && !completed)
        {
            Invoke(nameof(SpawnPinkColorCore), 1.2f);
            completed = true;
        }
    }

    public void Create()
    {
        pinkDungeon = generator.PinkDungeon;
        CreateAndSetPositions();
    }

    private void CreateAndSetPositions()
    {
        // Inicializace hlavního bosse
        pinkMushroomBoss = Instantiate(this.pinkMushroomBoss, this.pinkMushroom.transform.position,
                this.pinkMushroom.transform.rotation);
        generator.setCharacterToCenterOfRandomRoom(pinkMushroomBoss, pinkDungeon, 2, 2);
        generator.Player.transform.position = pinkMushroomBoss.transform.position + new Vector3(3, 3);

        // Seznam konfigurací pro generování objektù specifikovaných typem Item
        var itemConfigs = new List<(Item prefab, Action<Item> positionSetter, int count)>()
        {
            (this.pinkRock2large, obj => generator.SetLargeItemToRoomCenter(obj, pinkDungeon, 3, 3), 2),
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
        (this.pinkMushroom, obj => generator.setCharacterToRandomPosition(obj, pinkDungeon, 5), 10),
    };

        // Generování Item objektù
        foreach (var config in itemConfigs)
        {
            for (int i = 0; i < config.count; i++)
            {
                Item item = Instantiate(config.prefab);
                config.positionSetter(item);
            }
        }

        // Generování Enemy objektù
        foreach (var config in enemyConfigs)
        {
            for (int i = 0; i < config.count; i++)
            {
                EnemyMushroomPink enemy = Instantiate(config.prefab);
                config.positionSetter(enemy);
            }
        }
    }


    private void SpawnPinkColorCore()
    {
        pinkColorCore = Instantiate(this.pinkColorCore,
            pinkMushroomBoss.transform.position, this.pinkColorCore.transform.rotation);
    }
}
