using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueDungeon : DungeonBehaviour
{
    [SerializeField]
    DungeonGenerator generator;

    [SerializeField]
    BlueSlime blueSlime;

    [SerializeField]
    BlueBossSlime blueBossSlime;

    [SerializeField]
    Item iceObstacle1;

    [SerializeField]
    Item iceObstacle2;

    [SerializeField]
    ColorCore blueColorCore;

    [SerializeField]
    Player player;

    [SerializeField]
    ParticleSystem snowGenerator;

    public BlueBossSlime BlueBossSlimeInstance { get; set; }

    Dungeon blueDungeon;

    private void Update()
    {
        if (IsPlayerInsideDungeon())
        {
            StartSnowEffect();
        }
        else
        {
            StopSnowEffect();
        }
        if (BlueBossSlimeInstance != null)
        {
            if (!BlueBossSlimeInstance.isAlive && !Completed)
            {
                StartCoroutine(CallSpawnColorCoreAfterDelay(1.2f, blueColorCore, BlueBossSlimeInstance.transform));
                Completed = true;
            }
        }
    }

    private void OnDrawGizmos()
    {
        //Gizmos.color = Color.green;
        //Gizmos.DrawWireCube(blueDungeon.DungeonBounds.center, blueDungeon.DungeonBounds.size);

        //if (player != null)
        //{
        //    Gizmos.color = Color.red;
        //    Gizmos.DrawSphere(player.transform.position, 0.5f);
        //}
    }

    public override void Create()
    {
        blueDungeon = generator.BlueDungeon;
        CreateAndSetPositions();

        // Inicializace pozice a velikosti particle systému
        snowGenerator.transform.position = new Vector3(
            blueDungeon.DungeonBounds.center.x,
            blueDungeon.DungeonBounds.yMax,
            0
        );

        var shape = snowGenerator.shape;
        shape.scale = new Vector3(
            blueDungeon.DungeonBounds.size.x,
            1,
            1
        );
    }

    public override void CreateAndSetPositions()
    {
        // Inicializace hlavního bosse
        BlueBossSlimeInstance = Instantiate(this.blueBossSlime, this.blueBossSlime.transform.position,
                this.blueBossSlime.transform.rotation);
        generator.setBossToRandomRoom(BlueBossSlimeInstance, blueDungeon, 2, 2);

        // Seznam konfigurací pro generování objektù specifikovaných typem Item
        var itemConfigs = new List<(Item prefab, Action<Item> positionSetter, int count)>()
        {
            (this.iceObstacle1, obj => generator.SetLargeItemToRandomPosition(obj, blueDungeon, 2, 3, 1), 10),
            (this.iceObstacle2, obj => generator.SetLargeItemToRandomPosition(obj, blueDungeon,2, 3, 1), 10),
        };

        var enemyConfigs = new List<(BlueSlime prefab, Action<BlueSlime> positionSetter, int count)>()
        {
            (this.blueSlime, obj => generator.setCharacterToRandomPosition(obj, blueDungeon, 0), 30),
        };

        GenerateDungeonObjects(itemConfigs, enemyConfigs);
    }

    private bool IsPlayerInsideDungeon()
    {
        Vector3Int playerPosition = Vector3Int.FloorToInt(player.transform.position);

        return playerPosition.x >= blueDungeon.DungeonBounds.xMin &&
               playerPosition.x < blueDungeon.DungeonBounds.xMax &&
               playerPosition.y >= blueDungeon.DungeonBounds.yMin &&
               playerPosition.y < blueDungeon.DungeonBounds.yMax;
    }

    private void StartSnowEffect()
    {
        Vector3 dungeonTopCenter = new Vector3(
            blueDungeon.DungeonBounds.center.x,
            blueDungeon.DungeonBounds.yMax,
            0
        );
        snowGenerator.transform.position = dungeonTopCenter;

        var shape = snowGenerator.shape;
        shape.scale = new Vector3(
            blueDungeon.DungeonBounds.size.x,
            1,
            1
        );

        if (!snowGenerator.isPlaying)
        {
            snowGenerator.Play();
        }
    }

    private void StopSnowEffect()
    {
        if (snowGenerator.isPlaying)
        {
            snowGenerator.Stop();
        }
    }
}
