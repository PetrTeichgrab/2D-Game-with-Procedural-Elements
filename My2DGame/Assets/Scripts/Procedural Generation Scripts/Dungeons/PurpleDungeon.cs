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

        foreach (var room in purpleDungeon.RoomList)
        {
            float roomRatio = (float)room.FloorList.Count / purpleDungeon.Floor.FloorList.Count;

            int smallMushrooms = Mathf.RoundToInt(20 * roomRatio);
            int mediumMushrooms = Mathf.RoundToInt(10 * roomRatio);
            int largeMushrooms = Mathf.RoundToInt(5 * roomRatio);

            for (int i = 0; i < smallMushrooms; i++)
            {
                var item = Instantiate(purpleMushroomSmall);
                generator.SetItemToRoomPosition(item, room, 1, 1, 0, 1);
            }

            for (int i = 0; i < mediumMushrooms; i++)
            {
                var item = Instantiate(purpleMushroomMed);
                generator.SetItemToRoomPosition(item, room, 1, 1, 0, 1);
            }

            for (int i = 0; i < largeMushrooms; i++)
            {
                var item = Instantiate(purpleMushroomLarge);
                generator.SetItemToRoomPosition(item, room, 1, 1, 0, 1);
            }

            for (int i = 0; i < Mathf.RoundToInt(5 * roomRatio); i++)
            {
                var item = Instantiate(yellowMushroomMed);
                generator.SetItemToRoomPosition(item, room, 1, 1, 0, 1);
            }

            for (int i = 0; i < Mathf.RoundToInt(8 * roomRatio); i++)
            {
                var item = Instantiate(yellowMushroomSmall);
                generator.SetItemToRoomPosition(item, room, 1, 1, 0, 1);
            }
        }

        Debug.Log("Purple dungeon objekty vygenerovány podle velikosti místností.");
    }

}

