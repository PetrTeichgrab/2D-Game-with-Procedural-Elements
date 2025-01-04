using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenDungeon : DungeonBehaviour
{
    [SerializeField] private DungeonGenerator generator;

    [SerializeField] private GreenSlime greenSlime;

    [SerializeField] private Item treeBright1, treeBright2, treeBright3, treeBright4, treeBright5, treeBright6;
    [SerializeField] private Item treeDark1, treeDark2, treeDark3, treeDark4, treeDark5, treeDark6, treeDark7, treeDark8, treeDark9, treeDark10;
    [SerializeField] private Item bush1, bush2, bush3, bush4, bush5;

    [SerializeField] private Player player;

    private Dungeon greenDungeon;
    private ObjectPool<Item> objectPool;

    public override void Create()
    {
        greenDungeon = generator.GreenDungeon;
        CreateAndSetPositions();
    }

    public override void CreateAndSetPositions()
    {
        InitializePools();

        // Generov�n� statick�ch ��st� dungeonu
        GenerateStaticDungeon();

        // Spu�t�n� dynamick�ho generov�n� b�hem hry
        StartCoroutine(GenerateDynamicDungeon());
    }

    private void InitializePools()
    {
        var prefabs = new Dictionary<string, Item>
        {
            { "TreeBright1", treeBright1 },
            { "TreeBright2", treeBright2 },
            { "TreeBright3", treeBright3 },
            { "TreeBright4", treeBright4 },
            { "TreeBright5", treeBright5 },
            { "TreeBright6", treeBright6 },
            { "TreeDark1", treeDark1 },
            { "TreeDark2", treeDark2 },
            { "TreeDark3", treeDark3 },
            { "TreeDark4", treeDark4 },
            { "TreeDark5", treeDark5 },
            { "TreeDark6", treeDark6 },
            { "TreeDark7", treeDark7 },
            { "TreeDark8", treeDark8 },
            { "TreeDark9", treeDark9 },
            { "TreeDark10", treeDark10 },
            { "Bush1", bush1 },
            { "Bush2", bush2 },
            { "Bush3", bush3 },
            { "Bush4", bush4 },
            { "Bush5", bush5 },
        };

        objectPool = new ObjectPool<Item>(prefabs, 20);
    }

    private void GenerateStaticDungeon()
    {
        generator.Player.transform.position = new Vector3(greenDungeon.RoomList[0].Center.x, greenDungeon.RoomList[0].Center.y);

        foreach (var room in greenDungeon.RoomList)
        {
            bool isBrightTreeType = UnityEngine.Random.value > 0.5f;
            var selectedTreeType = isBrightTreeType
                ? new[] { "TreeBright1", "TreeBright2", "TreeBright3", "TreeBright4", "TreeBright5", "TreeBright6" }
                : new[] { "TreeDark1", "TreeDark2", "TreeDark3", "TreeDark4", "TreeDark5", "TreeDark6", "TreeDark7", "TreeDark8", "TreeDark9", "TreeDark10" };

            foreach (var type in selectedTreeType)
            {
                int itemCount = Mathf.RoundToInt((float)25 * room.FloorList.Count / greenDungeon.Floor.FloorList.Count);

                for (int i = 0; i < itemCount; i++)
                {
                    var item = objectPool.Get(type);
                    int height = isBrightTreeType ? 3 : 2;
                    generator.SetItemToRoomPosition(item, room, 2, height, 0, 1);
                }
            }
        }

        Debug.Log("Statick� ��sti dungeonu byly vygenerov�ny.");
    }

    private IEnumerator GenerateDynamicDungeon()
    {
        int totalFloorSize = greenDungeon.Floor.FloorList.Count;

        foreach (var room in greenDungeon.RoomList)
        {
            // Dynamicky generujeme ke�e
            foreach (var type in new[] { "Bush1", "Bush2", "Bush3", "Bush4", "Bush5" })
            {
                int bushCount = Mathf.RoundToInt((float)60 * room.FloorList.Count / totalFloorSize);

                for (int i = 0; i < bushCount; i++)
                {
                    var bush = objectPool.Get(type);

                    if (generator.SetItemToRoomPosition(bush, room, 1, 1, 1, 0) > 0.7f)
                    {
                        objectPool.Return(bush, type);
                        break;
                    }

                    yield return new WaitForSeconds(0.1f);
                }
            }

            yield return new WaitForSeconds(0.5f);
        }

        Debug.Log("Dynamick� ��sti dungeonu byly vygenerov�ny.");
    }
}
