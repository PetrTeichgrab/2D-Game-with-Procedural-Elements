using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor.U2D.Aseprite;
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
    Item treeDark8;

    [SerializeField]
    Item treeDark9;

    [SerializeField]
    Item treeDark10;

    [SerializeField]
    Item treeDark11;

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

    [SerializeField]
    Player player;

    Dungeon greenDungeon;

    bool isGenerated;

    public override void Create()
    {
        greenDungeon = generator.GreenDungeon;
        CreateAndSetPositions();
    }

    public override void CreateAndSetPositions()
    {
        StartCoroutine(GenerateDungeon());
    }

    private IEnumerator GenerateDungeon()
    {
        generator.Player.transform.position = new Vector3(greenDungeon.RoomList[0].Center.x, greenDungeon.RoomList[0].Center.y);

        var brightTrees = new List<(Item prefab, int baseCount, int width, int height, int offset)>
    {
        (this.treeBright1, 50, 2, 2, 2),
        (this.treeBright2, 50, 2, 2, 2),
        (this.treeBright3, 50, 2, 2, 2),
        (this.treeBright4, 50, 2, 2, 2),
        (this.treeBright5, 50, 2, 2, 2),
        (this.treeBright6, 50, 2, 2, 2),
    };

        var darkTrees = new List<(Item prefab, int baseCount, int width, int height, int offset)>
    {
        (this.treeDark1, 30, 2, 2, 2),
        (this.treeDark2, 30, 2, 2, 2),
        (this.treeDark3, 30, 2, 2, 2),
        (this.treeDark4, 30, 2, 2, 2),
        (this.treeDark5, 30, 2, 2, 2),
        (this.treeDark6, 30, 2, 2, 2),
        (this.treeDark7, 30, 2, 2, 2),
        (this.treeDark8, 30, 2, 2, 2),
        (this.treeDark9, 30, 2, 2, 2),
        (this.treeDark10, 30, 2, 2, 2),
    };

        var bushes = new List<(Item prefab, int baseCount, int offset)>
    {
        (this.bush1, 100, 0),
        (this.bush2, 100, 0),
        (this.bush3, 100, 0),
        (this.bush4, 100, 0),
        (this.bush5, 100, 0),
    };

        int totalFloorSize = greenDungeon.Floor.FloorList.Count;

        foreach (var room in greenDungeon.RoomList)
        {
            int roomSize = room.FloorList.Count;

            var selectedTreeType = UnityEngine.Random.value > 0.5f ? brightTrees : darkTrees;

            foreach (var (prefab, baseCount, width, height, offset) in selectedTreeType)
            {
                int itemCount = Mathf.Max(1, Mathf.RoundToInt((float)baseCount * roomSize / totalFloorSize));

                for (int i = 0; i < itemCount; i++)
                {
                    var item = Instantiate(prefab);

                    // Kontrola zaplnìní místnosti
                    if (generator.SetItemToRoomPosition(item, room, width, height, offset) > 0.60f)
                    {
                        Debug.Log($"Pøeskakuji místnost, zaplnìno na více než 60 %.");
                        break;
                    }

                    yield return null; // Poèkat 1 snímek
                }
            }

            foreach (var (prefab, baseCount, offset) in bushes)
            {
                int bushCount = Mathf.Max(1, Mathf.RoundToInt((float)baseCount * roomSize / totalFloorSize));

                for (int i = 0; i < bushCount; i++)
                {
                    var bush = Instantiate(prefab);

                    // Kontrola zaplnìní místnosti
                    if (generator.SetItemToRoomPosition(bush, room, 1, 1, offset) > 0.70f)
                    {
                        Debug.Log($"Pøeskakuji místnost pøi generování keøù, zaplnìno na více než 70 %.");
                        break;
                    }

                    yield return null; // Poèkat 1 snímek
                }
            }

            yield return null; // Poèkat 1 snímek po dokonèení místnosti
        }

        Debug.Log("Dungeon byl kompletnì vygenerován.");
    }


}
