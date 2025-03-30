using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PurpleDungeon : DungeonBehaviour
{
    [SerializeField]
    DungeonGenerator generator;

    Dungeon purpleDungeon;

    [SerializeField]
    Item purpleMushroomSmall;

    [SerializeField]
    Item purpleMushroomMed;

    [SerializeField]
    Item purpleMushroomLarge;

    [SerializeField]
    Item yellowMushroomSmall;

    [SerializeField]
    Item yellowMushroomMed;

    [SerializeField]
    ColorCore purpleColorCore;

    [SerializeField]
    ColorCore purpleColorCoreInstance;

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
        purpleColorCoreInstance = Instantiate(purpleColorCore);
        generator.SetItemToRandomPosition(purpleColorCoreInstance, purpleDungeon, 1);

        portal = Instantiate(portalPrefab);
        generator.SetLargeItemToRandomPosition(portal, purpleDungeon, 2, 2, 1);

        portal2 = Instantiate(portalPrefab);
        generator.SetLargeItemToRandomPosition(portal2, purpleDungeon, 2, 2, 1);

        foreach (var room in purpleDungeon.RoomList)
        {
            int roomSize = room.FloorList.Count;
            int dungeonSize = purpleDungeon.Floor.FloorList.Count;
            float roomRatio = (float)roomSize / dungeonSize;

            List<(Item prefab, int width, int height)> itemPool = new List<(Item, int, int)>();

            int smallCount = Mathf.RoundToInt(50 * roomRatio);
            int mediumCount = Mathf.RoundToInt(35 * roomRatio);
            int largeCount = Mathf.RoundToInt(25 * roomRatio);
            int yellowSmallCount = Mathf.RoundToInt(45 * roomRatio);
            int yellowMediumCount = Mathf.RoundToInt(35 * roomRatio);

            for (int i = 0; i < smallCount; i++) itemPool.Add((purpleMushroomSmall, 1, 1));
            for (int i = 0; i < mediumCount; i++) itemPool.Add((purpleMushroomMed, 1, 1));
            for (int i = 0; i < largeCount; i++) itemPool.Add((purpleMushroomLarge, 2, 2));
            for (int i = 0; i < yellowSmallCount; i++) itemPool.Add((yellowMushroomSmall, 1, 1));
            for (int i = 0; i < yellowMediumCount; i++) itemPool.Add((yellowMushroomMed, 1, 1));

            for (int i = 0; i < itemPool.Count; i++)
            {
                int randomIndex = UnityEngine.Random.Range(i, itemPool.Count);
                (itemPool[i], itemPool[randomIndex]) = (itemPool[randomIndex], itemPool[i]);
            }

            foreach (var (prefab, width, height) in itemPool)
            {
                var item = Instantiate(prefab);
                generator.SetItemToRoomPosition(item, room, width, height, 1, 1);
            }
        }

        generator.Player.transform.position = new Vector3(portal.Position.x + 1, portal.Position.y + 1);
        Debug.Log("Purple dungeon objekty vygenerovány promíchanì jako v Green dungeon stylu.");
    }



}

