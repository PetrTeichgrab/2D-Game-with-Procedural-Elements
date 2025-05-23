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
        if (IsPlayerInsideDungeon(player, blueDungeon))
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
        if (Input.GetKeyDown(KeyCode.F2) && PlayerPrefs.GetInt("DevTools", 0) == 1)
        {
            generator.Player.transform.position = new Vector3(BlueBossSlimeInstance.Position.x + 5, BlueBossSlimeInstance.Position.y + 5);
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

        // Inicializace pozice a velikosti particle syst�mu
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
        portal = Instantiate(portalPrefab);
        generator.SetLargeItemToRandomPosition(portal, blueDungeon, 2, 2, 1);
        portal2 = Instantiate(portalPrefab);
        generator.SetLargeItemToRandomPosition(portal2, blueDungeon, 2, 2, 1);
        // Inicializace hlavn�ho bosse
        BlueBossSlimeInstance = Instantiate(this.blueBossSlime, this.blueBossSlime.transform.position,
                this.blueBossSlime.transform.rotation);
        generator.setBossToRandomRoom(BlueBossSlimeInstance, blueDungeon, 2, 2);

        // Seznam konfigurac� pro generov�n� objekt� specifikovan�ch typem Item
        var itemConfigs = new List<(Item prefab, Action<Item> positionSetter, int count)>()
        {
            (this.iceObstacle1, obj => generator.SetLargeItemToRandomPosition(obj, blueDungeon, 2, 3, 1), 10),
            (this.iceObstacle2, obj => generator.SetLargeItemToRandomPosition(obj, blueDungeon,2, 3, 1), 10),
        };

        var enemyConfigs = new List<(Character prefab, Action<Character> positionSetter)>()
        {
            (this.blueSlime, obj => generator.setCharacterToRandomPosition(obj, blueDungeon, 0)),
        };

        GenerateObjects(itemConfigs);
        GenerateEnemies(blueDungeon, enemyConfigs);

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
